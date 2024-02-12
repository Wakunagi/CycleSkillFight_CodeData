using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.Demo.SlotRacer.Utils;
using Photon.Realtime;
using Program.GameSystem.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Program.InGame.Battle {
    public class NW_FighterController : FighterController {

        [SerializeField] bool isPlayer = false;
        [SerializeField] FighterSkillData player_fighter, enemy_fighter;
        [SerializeField] FighterDatas fighter_data_list;
        [SerializeField] string sc_skill_select_name;

        NW_DataMessenger data_messenger = new NW_DataMessenger();

        protected override void Start() {
            if (!isPlayer) return;
            GameObject fc = PhotonNetwork.Instantiate("FighterController", Vector3.zero, Quaternion.identity);
            data_messenger = fc.GetComponent<NW_DataMessenger>();


            FighterStatus rs = player_fighter.status;
            string[] rss = player_fighter.skills;
            data_messenger.SetUP(
                rs.name, rs.life, rs.atk, rs.def, rs.spd, rs.job,
                rss[0], rss[1], rss[2], rss[3], rss[4], rss[5]
            );
        }

        //自分側での設定
        public void SetUP() {
            base.Start();
        }

        //相手側での設定
        public void SetUP(
            string name, float life, float atk, float def, float spd, string job,
            string skill0, string skill1, string skill2, string skill3, string skill4, string skill5) {

            FighterStatus fs = new FighterStatus();
            fs.name = name;
            fs.life = life;
            fs.atk = atk;
            fs.def = def;
            fs.spd = spd;
            fs.job = job;
            fs.img = fighter_data_list.GetImage(job);

            enemy_fighter.SetFighterStatus(fs);

            enemy_fighter.SetSkill(0, skill0);
            enemy_fighter.SetSkill(1, skill1);
            enemy_fighter.SetSkill(2, skill2);
            enemy_fighter.SetSkill(3, skill3);
            enemy_fighter.SetSkill(4, skill4);
            enemy_fighter.SetSkill(5, skill5);

            base.Start();
        }

        //スキルの決定
        protected override void SelectSkill(AbilityType type) {
            if (isPlayer) {
                base.SelectSkill(type);
                data_messenger.SelectSkill(type);
            }
            else { base.DisplaySkillData(type); }
        }

        public void SelectSkill(int type) {
            base.SelectSkill(AbilityType.Low + type);
        }

        protected override void GameEnd() {
            if (isPlayer) { base.GameEnd(); }
        }

        public override void OnClick_GameEnd() {
            Debug.LogError("Scene change!");
            // ルームから退出する
            PhotonNetwork.LeaveRoom();
            // Photonのサーバーから切断する
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(sc_skill_select_name);
        }

    }
}