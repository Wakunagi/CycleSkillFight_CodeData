using Program.GameSystem;
using Program.GameSystem.Data;
using Program.InGame.Skill;
using Program.OutGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Program.InGame.Battle.FighterController;

namespace Program.InGame.Battle {

    public class FighterController : InputManager {

        //�f�[�^�擾�p
        [SerializeField] SkillDictionary database;
        [SerializeField] FighterSkillData skillData;

        //���X�N���v�g
        [SerializeField] BattleManager battleManager;

        //UI�֌W
        [SerializeField] GameObject select_panel;
        [SerializeField] protected SelectButtonData low_btn, middle_btn, high_btn, charge_btn;
        [SerializeField] Text fighter_name_text, low_snum_text, middle_snum_text;
        [SerializeField] Slider life_gauge, charge_gauge;
        [SerializeField]Text life_vol_text, charge_vol_text;
        //�Q�[���I�����p
        [SerializeField] GameObject end_panel;
        [SerializeField] Button end_btn;
        [SerializeField] Text win_lose_text;
        //�X�L���̐����p
        [SerializeField] Text sill_name_text, skill_explanation_text;
        //�X�L���̃A�j���[�V�����i�摜���t�F�[�h�����j
        [SerializeField] Image animation_img;

        //�`���[�W�i�X�L���j�֌W
        [SerializeField] GameObject chargeSkillObj_prefab;
        SkillParent charge_skill;

        //Player�֌W
        [SerializeField] SpriteRenderer fighter_image_sprite;
        [SerializeField] int pos = 0;

        List<SkillObjct> skillObjcts = new List<SkillObjct>();
        SelectButtonData[] select_background = new SelectButtonData[(int)AbilityType.End];

        AnyPhase myPhase = AnyPhase.End;
        FighterStatus myStatus = new FighterStatus();

        int player_id = 0;
        int low_count = 0, middle_count = 0, high_count = 0;

        float pos_y = 0;
        float life_max=0;

        AbilityType selectSkillPower = AbilityType.End;
        AbilityType clicked_skill_power = AbilityType.End;

        bool isGameEnd = false;
        bool isDecision = false; //���łɑI�����I�������


        protected virtual void Start() {

            //�e�X�L���𐶐�
            foreach (string name_id in skillData.skills) {
                SkillObjct skillObj = new SkillObjct();

                skillObj.name_id = name_id;
                skillObj.data = database.GetSkillData(name_id);
                skillObj.obj = Instantiate(skillObj.data.prefab);
                skillObj.skill = skillObj.obj.GetComponent<SkillParent>();

                if (skillObj.skill == null) { Debug.Log("Skill is not set."); return; }

                skillObj.skill.SetSkillData(skillObj.data);
                skillObj.skill.SetSkillPower(skillObj.data.skillPower);
                skillObj.skill.SetImage(animation_img);
                skillObj.skill.SetSprite(skillObj.data.image);
                skillObj.skill.SetBattleManager(battleManager);
                skillObjcts.Add(skillObj);
            }

            //�`���[�W�X�L�����쐬
            charge_skill = Instantiate(chargeSkillObj_prefab).GetComponent<SkillParent>();
            if (charge_skill == null) { Debug.Log("Skill is not set."); return; }

            charge_skill.SetImage(animation_img);
            charge_skill.SetBattleManager(battleManager);

            //�X�L���{�^���̊��蓖��
            select_background[(int)AbilityType.Low] = low_btn;
            select_background[(int)AbilityType.Middle] = middle_btn;
            select_background[(int)AbilityType.High] = high_btn;
            select_background[(int)AbilityType.Charge] = charge_btn;

            for (int i = 0; i < select_background.Length; i++) {
                SelectButtonData obj = select_background[i];
                AbilityType a = (AbilityType)i;
                obj.btn.onClick.AddListener(() => OnClick_SelectSkill(a));
            }

            //�X�L���f�[�^�̓ǂݍ���
            skillData.SetStatus(myStatus, skillData.status);

            fighter_name_text.text = myStatus.name;
            myStatus.pos = pos;
            player_id = battleManager.SetFighterStatus(myStatus);
            life_max = myStatus.life;
            life_gauge.maxValue = life_max;
            charge_gauge.maxValue = MyConst.CHARGE_MAX;
            fighter_image_sprite.sprite = myStatus.img;
            pos_y = transform.position.y;

            battleManager.PhaseSetUP(1);
        }


        private void Update() {

            PhaseChange(battleManager.phase);

            life_gauge.value = myStatus.life;
            charge_gauge.value = myStatus.charge;

            life_vol_text.text = (int)(myStatus.life *10)+"/"+life_max*10;
            charge_vol_text.text = myStatus.charge + "/"+MyConst.CHARGE_MAX;

            transform.position = new Vector2(myStatus.pos, pos_y);
        }

        protected virtual void GameEnd() {
            WinLose win_lose = battleManager.win_lose[player_id];
            if (win_lose == WinLose.Win) win_lose_text.text = "WIN";
            if (win_lose == WinLose.Lose) win_lose_text.text = "Lose";
            end_panel.gameObject.SetActive(true);
        }

        public virtual void OnClick_GameEnd() {
            if (isGameEnd) return;
            isGameEnd = true;
            sceneChangeManager.SceneChanger(1);
            end_btn.gameObject.SetActive(false);
        }

        //�t�F�C�Y�؂�ւ�
        void PhaseChange(AnyPhase phase) {
            if (phase == myPhase) return;
            myPhase = phase;

            switch (myPhase) {
                case AnyPhase.PreparationPhase: break;
                case AnyPhase.SelectPhase: SelectPhase_SetUp(); break;
                case AnyPhase.BattlePhase: break;
                case AnyPhase.EndPhase: EndPhase_SetUp(); break;

                case AnyPhase.End: GameEnd(); break;
                default: break;
            }
        }

        //�X�e�B�b�N����
        protected override void InputStick(InputPattern pattern) {

            switch (myPhase) {
                case AnyPhase.PreparationPhase: break;
                case AnyPhase.SelectPhase: SelectPhase_InputStick(pattern); break;
                case AnyPhase.BattlePhase: break;
                case AnyPhase.EndPhase: break;

                case AnyPhase.End: break;
                default: break;
            }
        }
        //�L�[�{�[�h����
        protected override void InputArrow(InputPattern pattern) {

            switch (myPhase) {
                case AnyPhase.PreparationPhase: break;
                case AnyPhase.SelectPhase: SelectPhase_InputAllow(pattern); break;
                case AnyPhase.BattlePhase: break;
                case AnyPhase.EndPhase: break;

                case AnyPhase.End: break;
                default: break;
            }
        }
        //����̓���
        protected override void InputDecision() {

            switch (myPhase) {
                case AnyPhase.PreparationPhase: break;
                case AnyPhase.SelectPhase:
                    // SelectPhase_InputDecision();
                    break;
                case AnyPhase.BattlePhase: break;
                case AnyPhase.EndPhase: break;

                case AnyPhase.End: OnClick_GameEnd(); break;
                default: break;
            }
        }


        void EndPhase_SetUp() {
            Debug.Log(myPhase);

            //�I�񂾃X�L���ɂ���Ď��̃X�L���ɕύX

            //Low�X�L��
            if (selectSkillPower == AbilityType.Low) {
                low_count = (low_count + 1) % MyConst.LOW_MAX;
                low_snum_text.text = (low_count + 1).ToString();
            }

            //Middle�X�L��
            else if (selectSkillPower == AbilityType.Middle) {
                middle_count = (middle_count + 1) % MyConst.MIDDLE_MAX;
                middle_snum_text.text = (middle_count + 1).ToString();
            }

            //High�X�L��
            else
            if (selectSkillPower == AbilityType.High) high_count = (high_count + 1) % MyConst.HIGH_MAX;
            battleManager.PhaseChange(1);
        }

        //�I���̃Z�b�g�A�b�v
        void SelectPhase_SetUp() {

            for (int i = 0; i < select_background.Length - 1; i++) { //charge�͍s��Ȃ�
                SelectButtonData obj = select_background[i];
                AbilityType type = (AbilityType)i;
                SkillObjct sobj = skillObjcts[GetSkillNum(type)];

                obj.hide.SetActive(sobj.skill.GetCharge_SkillPower(type) > myStatus.charge);
                obj.img.sprite = database.GetSkillData(skillObjcts[GetSkillNum(type)].name_id).image;
            }
            isDecision = false;
            select_background[(int)AbilityType.Charge].hide.SetActive(false);
            select_panel.SetActive(true);
            // SelectPhase_InputAllow(InputPattern.Down);

            AbilityType _type = InputPatternToAbilityType(InputPattern.DownArrow);
            if (_type < AbilityType.Charge && skillObjcts[GetSkillNum(_type)].skill.GetCharge_SkillPower(_type) > myStatus.charge) return;
            selectSkillPower = _type;
            SelectPhase_InputStick(InputPattern.Down);
        }


        //�X�L���I���̃X�e�B�b�N����
        void SelectPhase_InputStick(InputPattern pattern) {

            AbilityType type = InputPatternToAbilityType(pattern);
            DisplaySkillData(type);
        }

        //�X�L���I���̃{�^������
        void SelectPhase_InputAllow(InputPattern pattern) {

            AbilityType type = InputPatternToAbilityType(pattern);
            SelectSkill(type);
        }

        //�X�L���I����UI����
        void OnClick_SelectSkill(AbilityType type) {

            //1�x�ڂɉ����ꂽ��X�L���̓��e�̕\��
            if (clicked_skill_power != type) {
                clicked_skill_power = type;
                DisplaySkillData(type);
            }

            //2�x�ڂɉ����ꂽ��X�L���̌���
            else {
                SelectSkill(type);
                clicked_skill_power = AbilityType.End;
            }
        }

        //�X�L���̐����̕\��
        protected void DisplaySkillData(AbilityType type) {

            if (type == AbilityType.End || type > AbilityType.Charge) return;

            selectSkillPower = type;

            SetSelectBackground(selectSkillPower);
            //�`���[�W
            if (selectSkillPower == AbilityType.Charge) {
                charge_skill.SetExplanation(sill_name_text, skill_explanation_text);
            }
            else {

                skillObjcts[GetSkillNum(selectSkillPower)].skill.SetExplanation(sill_name_text, skill_explanation_text);

            }
        }

        //�X�L���̌���
        protected virtual void SelectSkill(AbilityType type) {

            if (type == AbilityType.End) return;
            if (type < AbilityType.Charge && skillObjcts[GetSkillNum(type)].skill.GetCharge_SkillPower(type) > myStatus.charge) return;

            selectSkillPower = type;

            if (isDecision) return;
            isDecision = true;

            //�`���[�W
            if (selectSkillPower == AbilityType.Charge) {
                battleManager.SelectSkill(charge_skill, player_id);
            }
            else {

                battleManager.SelectSkill(skillObjcts[GetSkillNum(selectSkillPower)].skill, player_id);

            }

            battleManager.PhaseChange(1);
            select_panel.SetActive(false);
        }



        //���͂���A�ǂ̃p���[�̃X�L�����𔻒�
        AbilityType InputPatternToAbilityType(InputPattern pattern) {

            //���FLow�X�L��
            if (pattern == InputPattern.Left ||
                pattern == InputPattern.LeftArrow) { return AbilityType.Low; }

            //��FMiddle�X�L��
            else
            if (pattern == InputPattern.Up ||
                pattern == InputPattern.UpArrow) { return AbilityType.Middle; }

            //�E�FHigh�X�L��
            else
            if (pattern == InputPattern.Right ||
                pattern == InputPattern.RightArrow) { return AbilityType.High; }

            //���F�`���[�W
            else
            if (pattern == InputPattern.Down ||
                pattern == InputPattern.DownArrow) { return AbilityType.Charge; }

            return AbilityType.End;
        }

        int GetSkillNum(AbilityType power) {
            if (power == AbilityType.Low) {
                return low_count;
            }
            else
            if (power == AbilityType.Middle) {
                return MyConst.LOW_MAX + middle_count;
            }
            else
            if (power == AbilityType.High) {
                return MyConst.LOW_MAX + MyConst.MIDDLE_MAX + high_count;
            }
            else {
                return -1;
            }
        }

        //�I��w�i�̕ύX
        void SetSelectBackground(AbilityType power) {
            foreach (SelectButtonData obj in select_background) obj.choice.SetActive(false);
            select_background[(int)power].choice.SetActive(true);
        }



        [System.Serializable]
        public class SelectButtonData {
            public Image img = null;
            public Button btn = null;
            public GameObject hide = null;
            public GameObject choice = null;
        }

        [System.Serializable]
        public class SkillObjct {
            public string name_id = null;
            public SkillData data = null;
            public GameObject obj = null;
            public SkillParent skill = null;
        }


    }

}