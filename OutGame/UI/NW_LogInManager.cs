using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NW_LogInManager : MonoBehaviourPunCallbacks {

    bool isJoined = false,
        isMatting = false;
    string battle_scene_name;

    private void Start() {
       //Screen.SetResolution(1200, 675, false);
    }

    private void Update() {
        if (isMatting) return;
        if (isJoined) {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount) {
                isMatting = true;
                Debug.Log("LoadScene");
                SceneManager.LoadScene(battle_scene_name);
            }
        }
    }

    public void OnlineSceneChange(string sn) {
        battle_scene_name = sn;
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();

    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        //ランダムで部屋に入る
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        isJoined = true;
        Debug.Log("joined");
    }

    //部屋がなければ作る
    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void DisConnected() {
        isJoined = false;
        // ルームから退出する
        PhotonNetwork.LeaveRoom();
        // Photonのサーバーから切断する
        PhotonNetwork.Disconnect();
    }
}