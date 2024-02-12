using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using Unity.VisualScripting;

using Program.GameSystem;
using Program.GameSystem.Data;


namespace Program.OutGame.UI {

    public class SkillSelectController : InputManager {

        [field: SerializeField] public FighterSkillData fighter_data { private set; get; }
        [SerializeField] private SkillDictionary skill_dictionry;
        [SerializeField] private FighterDatas fighter_dictionry;
        [SerializeField] private GameObject select_sbtn_parent, select_sbtn_prefab;
        [SerializeField] private GameObject player_sbtn_parent, player_sbtn_prefab;

        [SerializeField] private GameObject select_skill_data_panel;

        private List<Select_SButtonData> select_sbtn_data_list = new List<Select_SButtonData>();
        private List<Player_SButtonData> player_sbtn_data_list = new List<Player_SButtonData>();

        [SerializeField] private Scrollbar sbtn_scrollbar;
        int display_select_sbtn_count = 0;

        int choice_player_sbtn_num;

        int choice_select_sbtn = 0;
        int choice_player_sbtn = 0;

        int clicked_sbtn_num = 0;

        int[] all_power_skill_count = new int[(int)SkillPower.End];
        SkillPower isDisplaySelectPanel = SkillPower.End;

        [SerializeField] GameObject play_start_panel;
        [SerializeField] GameObject play_start_choice_obj;
        [SerializeField] GameObject fighters_choice_obj;
        [SerializeField] Text fighters_text,fighter_status_text;

        List<GameObject> select_sbtn_choice_obj_list = new List<GameObject>();
        List<GameObject> player_sbtn_choice_obj_list = new List<GameObject>();

        int fighter_data_choice_num = 0;

        bool isOnPlayStartBtn = false;
        bool isOnFighterBtn = false;
        bool isDecision = false;

        [SerializeField]Text skill_name_text, skill_explanation_text;

        [SerializeField] GridLayoutGroup player_sbtn_group;


        void Start() {
            //�e�p���[�̃X�L���̌���������           
            for (int i = 0; i < all_power_skill_count.Length; i++) all_power_skill_count[i] = 0;

            player_sbtn_choice_obj_list.Add(play_start_choice_obj);
            player_sbtn_choice_obj_list.Add(fighters_choice_obj);

            SetSelectSButtons();
            SetPlayerSButtons();
            OnClick_DisplaySelectPanel(0);

            float x = player_sbtn_group.cellSize.x / 1920 * Screen.width;
            float y = player_sbtn_group.cellSize.y / 1080 * Screen.height;
            player_sbtn_group.cellSize = new Vector2(x, y);

            x = player_sbtn_group.spacing.x / 1920 * Screen.width;
            y = player_sbtn_group.spacing.y / 1080 * Screen.height;
            player_sbtn_group.spacing = new Vector2(x, y);

            isDisplaySelectPanel = SkillPower.End;
            select_skill_data_panel.SetActive(false);
            play_start_panel.SetActive(false);
            DisplayChoiceObj(player_sbtn_data_list[0].choice, player_sbtn_choice_obj_list);
            OnClick_FighterSetter(0);
        }

        //PlayerButton�̏����ݒ�
        void SetPlayerSButtons() {
            for (int i = 0; i < MyConst.SKILL_MAX; i++) {

                GameObject data_obj = Instantiate(player_sbtn_prefab);
                data_obj.transform.SetParent(player_sbtn_parent.transform);

                //Player_SButton�ւ̊��蓖��
                Player_SButtonData btn_data = data_obj.GetComponent<Player_SButtonData>();

                //�G���[����
                if ((btn_data.img == null) || (btn_data.btn == null) || (btn_data.choice == null)) {
                    Destroy(data_obj);
                    Debug.LogError("data_obj does not have Component.");
                    return;
                }

                //�摜�ύX
                SkillData data = skill_dictionry.GetSkillData(fighter_data.skills[i]);
                if (data == null) {
                    foreach (SkillData skillData in skill_dictionry.datas)
                        if (skillData.skillPower == GetSkillPower(i)) {
                            fighter_data.SetSkill(i, skillData.name_id);
                            data = skillData;
                        }
                }
                btn_data.img.sprite = data.image;

                //�{�^���ւ̊��蓖��
                int cycle_num = i;
                btn_data.btn.onClick.AddListener(() => OnClick_DisplaySelectPanel(cycle_num));

                //�i���o�[�e�L�X�g��ύX
                int cycle_num_in_power = cycle_num + 1;
                if (cycle_num_in_power > MyConst.LOW_MAX) {
                    cycle_num_in_power -= MyConst.LOW_MAX;
                    if (cycle_num_in_power > MyConst.MIDDLE_MAX) cycle_num_in_power -= MyConst.MIDDLE_MAX;
                }
                btn_data.num_text.text = cycle_num_in_power.ToString();

                //�{�^���̓o�^
                player_sbtn_data_list.Add(btn_data);

                //�Z���N�g���ɕ\������I�u�W�F�N�g�����X�g�֓o�^
                player_sbtn_choice_obj_list.Add(btn_data.choice);
            }
        }

        //SkillButton�̏����ݒ�
        void SetSelectSButtons() {
            for (int i = 0; i < skill_dictionry.datas.Count; i++) {
                SkillData data = skill_dictionry.datas[i];

                GameObject data_obj = Instantiate(select_sbtn_prefab);
                data_obj.transform.SetParent(select_sbtn_parent.transform);

                //SButton�ւ̊��蓖��
                Select_SButtonData btn_data = new Select_SButtonData();
                btn_data.obj = data_obj;
                btn_data.img = data_obj.transform.GetChild(MyConst.SKILL_IMAGE).GetComponent<Image>();
                btn_data.btn = data_obj.GetComponent<Button>();
                btn_data.choice = data_obj.transform.GetChild(MyConst.CHOICE_IMAGE).gameObject;
                btn_data.data = data;

                //�G���[����
                if ((btn_data.img == null) || (btn_data.btn == null) || (btn_data.choice == null)) {
                    Destroy(data_obj);
                    Debug.LogError("data_obj does not have Component.");
                    return;
                }

                //�摜�ύX
                btn_data.img.sprite = data.image;

                //�{�^���̐ݒ�
                int a = i;
                btn_data.btn.onClick.AddListener(() => OnClick_SelectSkillButton(a));

                //�e�p���[�̃X�L���̌�
                all_power_skill_count[(int)data.skillPower]++;

                //�{�^���̓o�^
                select_sbtn_data_list.Add(btn_data);

                //�Z���N�g���ɕ\������I�u�W�F�N�g�����X�g�֓o�^
                select_sbtn_choice_obj_list.Add(btn_data.choice);

            }
        }


        //���E�̓���
        protected override void InputStick(InputPattern pattern) {
            if (isDecision) return;

            int num = 0;

            //�X�L���I��
            if (isDisplaySelectPanel != SkillPower.End) {

                if (pattern == InputPattern.Right) num = 1;
                else if (pattern == InputPattern.Left) num = -1;
                else return;


                if (choice_select_sbtn + num < 0) { return; }
                if (choice_select_sbtn + num > (all_power_skill_count[(int)isDisplaySelectPanel] - 1)) { return; }

                choice_select_sbtn += num;

                Debug.Log("Num : "+num+" Bar : " + (float)((float)num / (float)display_select_sbtn_count) + " count : " + display_select_sbtn_count);

                sbtn_scrollbar.value += (float)((float)num / (float)(display_select_sbtn_count-1));

                //�����̕ύX
                SetSelectSkillDataToDisplay(GetSkillNumInPower(isDisplaySelectPanel, choice_select_sbtn));
            }

            //�ǂ̃X�L����ύX���邩��I��
            else {
                if (pattern == InputPattern.Right) num = 1;
                else if (pattern == InputPattern.Left) num = -1;
                else if (pattern == InputPattern.Up) num = -3;
                else if (pattern == InputPattern.Down) num = 3;
                else return;

                if (isOnFighterBtn) {
                    if ((pattern == InputPattern.Right) || (pattern == InputPattern.Left)) {
                        OnClick_FighterSetter(num);
                        return;
                    }
                    else if ((pattern == InputPattern.Down)) num = 1;
                }

                if (choice_player_sbtn + num < 0) {
                    choice_player_sbtn = -1;
                    isOnFighterBtn = true;
                    DisplayChoiceObj(fighters_choice_obj, player_sbtn_choice_obj_list);
                    OnClick_FighterSetter(0);
                    return;
                }
                if (choice_player_sbtn + num > MyConst.SKILL_MAX - 1) {
                    choice_player_sbtn = MyConst.SKILL_MAX;
                    isOnPlayStartBtn = true;
                    DisplayChoiceObj(play_start_choice_obj, player_sbtn_choice_obj_list);
                    return;
                }

                isOnPlayStartBtn = false;
                isOnFighterBtn = false;
                choice_player_sbtn += num;

                DisplayChoiceObj(player_sbtn_data_list[choice_player_sbtn].choice, player_sbtn_choice_obj_list);
            }

        }

        //����̓���
        protected override void InputDecision() {

            if (isOnFighterBtn) return;

            if (isOnPlayStartBtn) {
                OnClick_PlayStart();
                return;
            }

            if (isDisplaySelectPanel != SkillPower.End) {
                int snum = GetSkillNumInPower(isDisplaySelectPanel, choice_select_sbtn);
                if (snum < 0) { Debug.Log("Skill is not found."); return; }
                SetTargetSkill(snum);
            }
            else {
                OnClick_DisplaySelectPanel(choice_player_sbtn);
            }

        }


        //fighter�̐ݒ�
        public void OnClick_FighterSetter(int delta) {
            fighter_data_choice_num+= delta;
            if(fighter_data_choice_num < 0)fighter_data_choice_num = fighter_dictionry.datas.Count-1;
            if (fighter_data_choice_num >= fighter_dictionry.datas.Count) fighter_data_choice_num = 0;

            fighter_data.SetFighterStatus( fighter_dictionry.datas[fighter_data_choice_num]);
            fighters_text.text = fighter_dictionry.datas[fighter_data_choice_num].name;

            string text =
                "���@�O�@ : " + fighter_dictionry.datas[fighter_data_choice_num].name + "\n"+
                "���C�t�@ : " + fighter_dictionry.datas[fighter_data_choice_num].life + "\n" +
                "�U���́@ : " + fighter_dictionry.datas[fighter_data_choice_num].atk + "\n" +
                "�h��́@ : " + fighter_dictionry.datas[fighter_data_choice_num].def + "\n" +
                "�X�s�[�h : " + fighter_dictionry.datas[fighter_data_choice_num].spd;

            fighter_status_text.text = text;

            DisplayChoiceObj(fighters_choice_obj, player_sbtn_choice_obj_list);
        }
        
        //�X�L���I����ʂ��J�����Ƃ�
        public void OnClick_DisplaySelectPanel(int cycleNum) {

            SkillPower skillPower = GetSkillPower(cycleNum);
            if (skillPower == SkillPower.End) return;

            display_select_sbtn_count = 0;

            //skillPower�������Ȃ�\��
            foreach (Select_SButtonData btn in select_sbtn_data_list) {
                if (btn.data.skillPower == skillPower) {
                    btn.obj.SetActive(true);
                    display_select_sbtn_count++;
                }
                else
                    btn.obj.SetActive(false);
            }

            //�I�𒆂̃X�L�����ǂꂩ��\������
            DisplayChoiceObj(select_sbtn_data_list[GetSkillNumInPower(skillPower, choice_select_sbtn)].choice, select_sbtn_choice_obj_list);

            choice_player_sbtn_num = cycleNum;
            isDisplaySelectPanel = skillPower;

            select_skill_data_panel.SetActive(true);

            Debug.Log("sbum in power : " + choice_select_sbtn);

            //�X�N���[���o�[�̒���
            sbtn_scrollbar.value = (float)((float)choice_select_sbtn / (float)(display_select_sbtn_count - 1));

            //�����̕ύX
            skill_name_text.text = select_sbtn_data_list[GetSkillNumInPower(isDisplaySelectPanel, choice_select_sbtn)].data.name;
            skill_explanation_text.text = select_sbtn_data_list[GetSkillNumInPower(isDisplaySelectPanel, choice_select_sbtn)].data.explanation;

            DisplayChoiceObj(player_sbtn_data_list[cycleNum].choice, player_sbtn_choice_obj_list);
        }

        //�eSelect_sbtn�������ꂽ���̏���
        public void OnClick_SelectSkillButton(int snum) {

            //1�x�ڂɉ����ꂽ��X�L���̓��e�̕\��
            if(clicked_sbtn_num != snum) {
                clicked_sbtn_num = snum;
                SetSelectSkillDataToDisplay(snum);
            }

            //2�x�ڂɉ����ꂽ��X�L���̌���
            else {
                clicked_sbtn_num = 0;
                SetTargetSkill(snum);
            }
        }

        //�X�L���I���̏I��
        public void OnClick_PlayStart() {
            DisplayChoiceObj(play_start_choice_obj, player_sbtn_choice_obj_list);
            isDecision = !isDecision;
            int n = 0;
            if (isDecision) { n = 1; play_start_panel.SetActive(true); }
            else { n = -1; play_start_panel.SetActive(false); }
            sceneChangeManager.SceneChanger(n);
        }


        //�X�L���̓��e��\������
        void SetSelectSkillDataToDisplay(int snum) {
            Debug.Log("Choice_Sbtn = " + choice_select_sbtn);
            //�����̕ύX
            skill_name_text.text = select_sbtn_data_list[snum].data.name;
            skill_explanation_text.text = select_sbtn_data_list[snum].data.explanation;

            //�I�𒆂̕\��
            DisplayChoiceObj(select_sbtn_data_list[snum].choice, select_sbtn_choice_obj_list);
        }

        //�X�L���̌���
        public void SetTargetSkill(int snum) {
            select_skill_data_panel.SetActive(false);
            Select_SButtonData sbtn_data = select_sbtn_data_list[snum];
            fighter_data.SetSkill(choice_player_sbtn_num, sbtn_data.data.name_id);
            player_sbtn_data_list[choice_player_sbtn_num].img.sprite = sbtn_data.img.sprite;
            isDisplaySelectPanel = SkillPower.End;
            choice_select_sbtn = 0;
        }


        //�X�L���p���[�̎擾
        public static SkillPower GetSkillPower(int power) {
            if (power < MyConst.LOW_MAX)
                return SkillPower.Low;
            else if (power < MyConst.LOW_MAX + MyConst.MIDDLE_MAX)
                return SkillPower.Middle;
            else if (power < MyConst.LOW_MAX + MyConst.MIDDLE_MAX + MyConst.HIGH_MAX)
                return SkillPower.High;
            else {
                Debug.Log("Power is Big." + power);
                return SkillPower.End;
            }
        }

        //�X�L���p���[�ɉ�����N�Ԗڂ̃X�L�����擾
        int GetSkillNumInPower(SkillPower spower, int n) {
            int scount = 0;
            for (int i = 0; i < select_sbtn_data_list.Count; i++) {
                if (select_sbtn_data_list[i].data.skillPower == spower) {
                    if (scount == n) return i;
                    scount++;
                }
            }
            return -1;
        }

        //�Z���N�g��\��
        void DisplayChoiceObj(GameObject obj, List<GameObject> hideList) {
            foreach (GameObject hideObj in hideList) hideObj.SetActive(false);
            obj.SetActive(true);
        }

    }
}