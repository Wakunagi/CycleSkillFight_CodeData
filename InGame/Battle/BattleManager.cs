using Program.GameSystem.Data;
using Program.InGame.Skill;
using Program.OutGame.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Program.InGame.Battle {
    public class BattleManager : MonoBehaviour {

        [field: SerializeField] public AnyPhase phase { private set; get; } = AnyPhase.PreparationPhase;
        int turn = 0;

        int decision_count = 0; //���̃t�F�C�Y�֍s���Ă����v���C���[�̐l��
        int order_count = 0;    //�X�L���̎g�p�����߂��v���C���[�̏���

        [SerializeField] FighterSkillSetting[] fighters = new FighterSkillSetting[MyConst.PLAYER_MAX];

        int fighters_count = 0;
        [SerializeField] int[] using_order; //�X�L���̎g�p��

        public WinLose[] win_lose { private set; get; } = new WinLose[MyConst.PLAYER_MAX];

        //�X�L��������X�L���̎g�p�i�A�j���[�V�����j�Ȃǂ��I���������m�点��p
        bool isCallBack = false;
        public void CallBack() { Debug.Log("CalllBack"); isCallBack = true; }

        [SerializeField] Text phase_text, turn_text;
        [SerializeField] float phaseChange_animation_time = 1.2f;


        void CameraPosChanger() {

            float pos_avg = 0, pos_y = 0;
            for (int i = 0; i < fighters.Length; i++) pos_avg += fighters[i].status.pos;

            for (int i = 0; i < fighters.Length; i++) fighters[i].status.pos -= (int)pos_avg;

            pos_avg = 0;
            for (int i = 0; i < fighters.Length; i++) pos_avg += fighters[i].status.pos;

            pos_avg /= fighters.Length;

            pos_y = math.abs(fighters[0].status.pos - pos_avg);
            Camera.main.transform.position = new Vector3(pos_avg, pos_y, -10);
            Camera.main.orthographicSize = pos_y + 0.5f;
        }

        //�t�@�C�^�[�̃X�e�[�^�X��ݒ�&�ԍ������蓖��
        public int SetFighterStatus(FighterStatus status) {
            int num = fighters_count;
            fighters_count++;

            win_lose[num] = WinLose.End;
            fighters[num] = new FighterSkillSetting();
            fighters[num].my_num = num;
            fighters[num].enemy_num = ElseFighterGetter(num);
            fighters[num].status = status;
            return num;
        }

        public void PhaseSetUP(int count) {
            decision_count += count;

            //�S���̃Z�b�g�A�b�v���I����PreparationPhase�i�����t�F�C�Y�j�Ɉڍs����B
            if (decision_count == MyConst.PLAYER_MAX) {
                decision_count = 0;
                PreparationPhase();
            }
        }

        /*
         * PreparationPhase -> Battlemanager����
         * SelectPhase      -> �v���C���[���̏���
         * BattlePhase      -> Battlemanager����
         * EndPhase         -> �v���C���[���̏���
        */

        //�t�F�C�Y�̐؂�ւ�
        public void PhaseChange(int count) {
            decision_count += count;

            //�S�������̊֐����ĂԂƐ؂�ւ���
            if (decision_count == MyConst.PLAYER_MAX) {
                PhaseChange();
            }
        }

        //�t�F�C�Y�̐؂�ւ�(����)
        public void PhaseChange() {

            phase++;
            if (phase == AnyPhase.End) phase = AnyPhase.PreparationPhase;

            //������
            decision_count = 0;
            order_count = 0;

            //�ǂ̃t�F�C�Y���ĂԂ�
            if (phase == AnyPhase.PreparationPhase) PreparationPhase();
            else if (phase == AnyPhase.BattlePhase) BattlePhase();
            else if (phase == AnyPhase.EndPhase) EndPhase();
        }

        //�����t�F�C�Y�i�^�[���J�E���g�j
        void PreparationPhase() {
            turn++;
            turn_text.text = turn.ToString();
            StartCoroutine(PreparationPhase_Animation());
        }
        IEnumerator PreparationPhase_Animation() {

            //�^�[�����n�܂������̃A�j���[�V����
            float c_alpha = phaseChange_animation_time;
            phase_text.text = "TURN " + turn;
            while (true) {
                c_alpha -= Time.deltaTime;
                Color white = new Color(1, 1, 1, c_alpha);
                phase_text.color = white;

                if (c_alpha <= 0) break;
                yield return null;
            }

            //�t�F�C�Y�؂�ւ�
            PhaseChange();
        }

        //���ꂪ�ǂ̃X�L�����g����?
        public void SelectSkill(SkillParent skill, int fNum) {
            fighters[fNum].skill = skill;
            fighters[fNum].order = order_count;
            order_count++;
        }

        //�X�L���̃L�����Z��
        public void SelectCancel(int fNum) {
            fighters[fNum].order = order_count;
            order_count++;
        }

        //�o�g���t�F�C�Y����
        public void BattlePhase() {
            StartCoroutine(BattlePhase_Animation());
        }
        IEnumerator BattlePhase_Animation() {

            //�o�g���t�F�C�Y���n�܂������̃A�j���[�V����
            float c_alpha = phaseChange_animation_time;
            phase_text.text = "BATTLE!";
            while (true) {
                c_alpha -= Time.deltaTime;
                Color white = new Color(1, 1, 1, c_alpha);
                phase_text.color = white;

                if (c_alpha <= 0) break;
                yield return null;
            }

            using_order = new int[MyConst.PLAYER_MAX];

            for (int i = 0; i < MyConst.PLAYER_MAX; i++) using_order[i] = fighters[i].my_num;

            //�X�L���g�p�����̕ύX
            for (int i = MyConst.PLAYER_MAX - 1; i > 0; i--) {
                for (int j = 0; j < i; j++) {
                    FighterSkillSetting j0 = fighters[j];
                    FighterSkillSetting j1 = fighters[j + 1];

                    //�D��x��j1�̕���������ΕύX
                    if (j0.skill.priority < j1.skill.priority) change();
                    
                    //�D��x�������Ȃ�
                    else
                    if (j0.skill.priority == j1.skill.priority) {

                        //j1�̕����X�s�[�h��������ΕύX
                        if (j0.status.spd < j1.status.spd) change();

                        //�X�s�[�h��������j1�̕�����ɑI�����Ă�����ύX
                        else
                        if (j0.status.spd == j1.status.spd) {
                            if (j0.order > j1.order) change();
                        }
                    }

                    //�D��x��j0�̂ق���������Ή������Ȃ�
                    
                    //�X�L���̎g�p�����̕ύX�֐�
                    void change() {
                        using_order[j] = fighters[j + 1].my_num;
                        using_order[j + 1] = fighters[j].my_num;
                    }
                }
            }

            //�v���C���[�S�����̃X�L���̎g�p
            for (int i = 0; i < MyConst.PLAYER_MAX; i++) {

                //�X�L���̎g�p�҂�ύX
                FighterSkillSetting fighter = fighters[using_order[i]];

                //�X�L���̎g�p
                isCallBack = false;
                fighter.skill.Ability(fighters[fighter.my_num].status, fighters[fighter.enemy_num].status);

                //�X�L���̏������A�j���[�V�������I���܂ő҂�
                while (!isCallBack) yield return null;

                //�ǂ��炩�̃v���C���[��HP��0�ɂȂ�����I��
                //���̌�̃X�L���̎g�p�͂����ɃG���h�t�F�C�Y��
                if (fighters[fighter.my_num].status.life < 0) {
                    win_lose[fighter.my_num] = WinLose.Lose;
                    win_lose[fighter.enemy_num] = WinLose.Win;
                    phase = AnyPhase.End;
                    yield break;
                }
                if (fighters[fighter.enemy_num].status.life < 0) {
                    win_lose[fighter.my_num] = WinLose.Win;
                    win_lose[fighter.enemy_num] = WinLose.Lose;
                    phase = AnyPhase.End;
                    yield break;
                }
            }

            //�t�F�C�Y�̐؂�ւ�
            PhaseChange();
        }

        //�G���h�t�F�C�Y����
        public void EndPhase() {

            //�G���h�t�F�C�Y�ɓK�p����X�L��������Ύg�p
            for (int i = 0; i < MyConst.PLAYER_MAX; i++) {
                FighterSkillSetting fighter = fighters[using_order[i]];
                fighter.skill.EndPhase(fighters[fighter.my_num].status, fighters[fighter.enemy_num].status);
                fighter.status.isDamaged = false;
            }

            //�ʒu�̕ύX
            CameraPosChanger();
        }

        //�G�v���C���[���擾
        public int ElseFighterGetter(int num) {
            if (num == 0) return 1;
            else if (num == 1) return 0;
            else return -1;
        }


    }
}