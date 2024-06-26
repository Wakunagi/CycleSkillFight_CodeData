using Photon.Pun;
using Program.GameSystem.Data;
using Program.InGame.Battle;
using UnityEngine;

public class NW_DataMessenger : MonoBehaviourPunCallbacks {

    [SerializeField] GameObject player;
    [SerializeField] NW_FighterController fighter_controller;


    //ステータスやスキルを全て引数として取得しておく
    public void SetUP(
            string name, float life, float atk, float def, float spd, string job,
            string skill0, string skill1, string skill2, string skill3, string skill4, string skill5) {

        MyPlayerSetUP();
        photonView.RPC(
            nameof(SendMyPlayerSetUP), RpcTarget.Others, 
            name, life, atk, def, spd, job, skill0, skill1, skill2, skill3, skill4, skill5);
    }

    //自分の端末側のセットアップ
    void MyPlayerSetUP() {
        player = GameObject.Find("LeftFighter");
        fighter_controller = player.GetComponent<NW_FighterController>();
        fighter_controller.SetUP();
    }

    //相手の端末側にステータスを伝える
    [PunRPC]
    void SendMyPlayerSetUP(
            string name, float life, float atk, float def, float spd, string job,
            string skill0, string skill1, string skill2, string skill3, string skill4, string skill5) {
        player = GameObject.Find("RightFighter");
        fighter_controller = player.GetComponent<NW_FighterController>();
        fighter_controller.SetUP(name, life, atk, def, spd, job, skill0, skill1, skill2, skill3, skill4, skill5);
    }

    //スキルの選択を相手の端末側に伝える
    public void SelectSkill(AbilityType type) {
        photonView.RPC(nameof(SendSelectSkill), RpcTarget.Others, (int)type);
    }
    [PunRPC]
    void SendSelectSkill(int type) {
        fighter_controller.SelectSkill(type);
    }

}