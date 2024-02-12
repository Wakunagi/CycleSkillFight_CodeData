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

        //データ取得用
        [SerializeField] SkillDictionary database;
        [SerializeField] FighterSkillData skillData;

        //他スクリプト
        [SerializeField] BattleManager battleManager;

        //UI関係
        [SerializeField] GameObject select_panel;
        [SerializeField] protected SelectButtonData low_btn, middle_btn, high_btn, charge_btn;
        [SerializeField] Text fighter_name_text, low_snum_text, middle_snum_text;
        [SerializeField] Slider life_gauge, charge_gauge;
        [SerializeField]Text life_vol_text, charge_vol_text;
        //ゲーム終了時用
        [SerializeField] GameObject end_panel;
        [SerializeField] Button end_btn;
        [SerializeField] Text win_lose_text;
        //スキルの説明用
        [SerializeField] Text sill_name_text, skill_explanation_text;
        //スキルのアニメーション（画像がフェードするやつ）
        [SerializeField] Image animation_img;

        //チャージ（スキル）関係
        [SerializeField] GameObject chargeSkillObj_prefab;
        SkillParent charge_skill;

        //Player関係
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
        bool isDecision = false; //すでに選択が終わったか


        protected virtual void Start() {

            //各スキルを生成
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

            //チャージスキルを作成
            charge_skill = Instantiate(chargeSkillObj_prefab).GetComponent<SkillParent>();
            if (charge_skill == null) { Debug.Log("Skill is not set."); return; }

            charge_skill.SetImage(animation_img);
            charge_skill.SetBattleManager(battleManager);

            //スキルボタンの割り当て
            select_background[(int)AbilityType.Low] = low_btn;
            select_background[(int)AbilityType.Middle] = middle_btn;
            select_background[(int)AbilityType.High] = high_btn;
            select_background[(int)AbilityType.Charge] = charge_btn;

            for (int i = 0; i < select_background.Length; i++) {
                SelectButtonData obj = select_background[i];
                AbilityType a = (AbilityType)i;
                obj.btn.onClick.AddListener(() => OnClick_SelectSkill(a));
            }

            //スキルデータの読み込み
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

        //フェイズ切り替え
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

        //スティック入力
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
        //キーボード入力
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
        //決定の入力
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

            //選んだスキルによって次のスキルに変更

            //Lowスキル
            if (selectSkillPower == AbilityType.Low) {
                low_count = (low_count + 1) % MyConst.LOW_MAX;
                low_snum_text.text = (low_count + 1).ToString();
            }

            //Middleスキル
            else if (selectSkillPower == AbilityType.Middle) {
                middle_count = (middle_count + 1) % MyConst.MIDDLE_MAX;
                middle_snum_text.text = (middle_count + 1).ToString();
            }

            //Highスキル
            else
            if (selectSkillPower == AbilityType.High) high_count = (high_count + 1) % MyConst.HIGH_MAX;
            battleManager.PhaseChange(1);
        }

        //選択のセットアップ
        void SelectPhase_SetUp() {

            for (int i = 0; i < select_background.Length - 1; i++) { //chargeは行わない
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


        //スキル選択のスティック入力
        void SelectPhase_InputStick(InputPattern pattern) {

            AbilityType type = InputPatternToAbilityType(pattern);
            DisplaySkillData(type);
        }

        //スキル選択のボタン入力
        void SelectPhase_InputAllow(InputPattern pattern) {

            AbilityType type = InputPatternToAbilityType(pattern);
            SelectSkill(type);
        }

        //スキル選択のUI入力
        void OnClick_SelectSkill(AbilityType type) {

            //1度目に押されたらスキルの内容の表示
            if (clicked_skill_power != type) {
                clicked_skill_power = type;
                DisplaySkillData(type);
            }

            //2度目に押されたらスキルの決定
            else {
                SelectSkill(type);
                clicked_skill_power = AbilityType.End;
            }
        }

        //スキルの説明の表示
        protected void DisplaySkillData(AbilityType type) {

            if (type == AbilityType.End || type > AbilityType.Charge) return;

            selectSkillPower = type;

            SetSelectBackground(selectSkillPower);
            //チャージ
            if (selectSkillPower == AbilityType.Charge) {
                charge_skill.SetExplanation(sill_name_text, skill_explanation_text);
            }
            else {

                skillObjcts[GetSkillNum(selectSkillPower)].skill.SetExplanation(sill_name_text, skill_explanation_text);

            }
        }

        //スキルの決定
        protected virtual void SelectSkill(AbilityType type) {

            if (type == AbilityType.End) return;
            if (type < AbilityType.Charge && skillObjcts[GetSkillNum(type)].skill.GetCharge_SkillPower(type) > myStatus.charge) return;

            selectSkillPower = type;

            if (isDecision) return;
            isDecision = true;

            //チャージ
            if (selectSkillPower == AbilityType.Charge) {
                battleManager.SelectSkill(charge_skill, player_id);
            }
            else {

                battleManager.SelectSkill(skillObjcts[GetSkillNum(selectSkillPower)].skill, player_id);

            }

            battleManager.PhaseChange(1);
            select_panel.SetActive(false);
        }



        //入力から、どのパワーのスキルかを判定
        AbilityType InputPatternToAbilityType(InputPattern pattern) {

            //左：Lowスキル
            if (pattern == InputPattern.Left ||
                pattern == InputPattern.LeftArrow) { return AbilityType.Low; }

            //上：Middleスキル
            else
            if (pattern == InputPattern.Up ||
                pattern == InputPattern.UpArrow) { return AbilityType.Middle; }

            //右：Highスキル
            else
            if (pattern == InputPattern.Right ||
                pattern == InputPattern.RightArrow) { return AbilityType.High; }

            //下：チャージ
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

        //選択背景の変更
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