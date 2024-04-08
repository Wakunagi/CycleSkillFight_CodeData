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

        int decision_count = 0; //次のフェイズへ行っていいプレイヤーの人数
        int order_count = 0;    //スキルの使用を決めたプレイヤーの順番

        [SerializeField] FighterSkillSetting[] fighters = new FighterSkillSetting[MyConst.PLAYER_MAX];

        int fighters_count = 0;
        [SerializeField] int[] using_order; //スキルの使用順

        public WinLose[] win_lose { private set; get; } = new WinLose[MyConst.PLAYER_MAX];

        //スキル側からスキルの使用（アニメーション）などが終わったかを知らせる用
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

        //ファイターのステータスを設定&番号を割り当て
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

            //全員のセットアップが終わるとPreparationPhase（準備フェイズ）に移行する。
            if (decision_count == MyConst.PLAYER_MAX) {
                decision_count = 0;
                PreparationPhase();
            }
        }

        /*
         * PreparationPhase -> Battlemanager処理
         * SelectPhase      -> プレイヤー側の処理
         * BattlePhase      -> Battlemanager処理
         * EndPhase         -> プレイヤー側の処理
        */

        //フェイズの切り替え
        public void PhaseChange(int count) {
            decision_count += count;

            //全員がこの関数を呼ぶと切り替える
            if (decision_count == MyConst.PLAYER_MAX) {
                PhaseChange();
            }
        }

        //フェイズの切り替え(内部)
        public void PhaseChange() {

            phase++;
            if (phase == AnyPhase.End) phase = AnyPhase.PreparationPhase;

            //初期化
            decision_count = 0;
            order_count = 0;

            //どのフェイズを呼ぶか
            if (phase == AnyPhase.PreparationPhase) PreparationPhase();
            else if (phase == AnyPhase.BattlePhase) BattlePhase();
            else if (phase == AnyPhase.EndPhase) EndPhase();
        }

        //準備フェイズ（ターンカウント）
        void PreparationPhase() {
            turn++;
            turn_text.text = turn.ToString();
            StartCoroutine(PreparationPhase_Animation());
        }
        IEnumerator PreparationPhase_Animation() {

            //ターンが始まった時のアニメーション
            float c_alpha = phaseChange_animation_time;
            phase_text.text = "TURN " + turn;
            while (true) {
                c_alpha -= Time.deltaTime;
                Color white = new Color(1, 1, 1, c_alpha);
                phase_text.color = white;

                if (c_alpha <= 0) break;
                yield return null;
            }

            //フェイズ切り替え
            PhaseChange();
        }

        //だれがどのスキルを使うか?
        public void SelectSkill(SkillParent skill, int fNum) {
            fighters[fNum].skill = skill;
            fighters[fNum].order = order_count;
            order_count++;
        }

        //スキルのキャンセル
        public void SelectCancel(int fNum) {
            fighters[fNum].order = order_count;
            order_count++;
        }

        //バトルフェイズ処理
        public void BattlePhase() {
            StartCoroutine(BattlePhase_Animation());
        }
        IEnumerator BattlePhase_Animation() {

            //バトルフェイズが始まった時のアニメーション
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

            //スキル使用順序の変更
            for (int i = MyConst.PLAYER_MAX - 1; i > 0; i--) {
                for (int j = 0; j < i; j++) {
                    FighterSkillSetting j0 = fighters[j];
                    FighterSkillSetting j1 = fighters[j + 1];

                    //優先度がj1の方が高ければ変更
                    if (j0.skill.priority < j1.skill.priority) change();
                    
                    //優先度が同じなら
                    else
                    if (j0.skill.priority == j1.skill.priority) {

                        //j1の方がスピードが高ければ変更
                        if (j0.status.spd < j1.status.spd) change();

                        //スピードが同じでj1の方が先に選択していたら変更
                        else
                        if (j0.status.spd == j1.status.spd) {
                            if (j0.order > j1.order) change();
                        }
                    }

                    //優先度がj0のほうが高ければ何もしない
                    
                    //スキルの使用順序の変更関数
                    void change() {
                        using_order[j] = fighters[j + 1].my_num;
                        using_order[j + 1] = fighters[j].my_num;
                    }
                }
            }

            //プレイヤー全員分のスキルの使用
            for (int i = 0; i < MyConst.PLAYER_MAX; i++) {

                //スキルの使用者を変更
                FighterSkillSetting fighter = fighters[using_order[i]];

                //スキルの使用
                isCallBack = false;
                fighter.skill.Ability(fighters[fighter.my_num].status, fighters[fighter.enemy_num].status);

                //スキルの処理＆アニメーションが終わるまで待つ
                while (!isCallBack) yield return null;

                //どちらかのプレイヤーのHPが0になったら終了
                //その後のスキルの使用はせずにエンドフェイズへ
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

            //フェイズの切り替え
            PhaseChange();
        }

        //エンドフェイズ処理
        public void EndPhase() {

            //エンドフェイズに適用するスキルがあれば使用
            for (int i = 0; i < MyConst.PLAYER_MAX; i++) {
                FighterSkillSetting fighter = fighters[using_order[i]];
                fighter.skill.EndPhase(fighters[fighter.my_num].status, fighters[fighter.enemy_num].status);
                fighter.status.isDamaged = false;
            }

            //位置の変更
            CameraPosChanger();
        }

        //敵プレイヤーを取得
        public int ElseFighterGetter(int num) {
            if (num == 0) return 1;
            else if (num == 1) return 0;
            else return -1;
        }


    }
}