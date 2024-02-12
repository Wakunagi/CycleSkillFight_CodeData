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
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster() {
        //�����_���ŕ����ɓ���
        PhotonNetwork.JoinRandomRoom();
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom() {
        isJoined = true;
        Debug.Log("joined");
    }

    //�������Ȃ���΍��
    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void DisConnected() {
        isJoined = false;
        // ���[������ޏo����
        PhotonNetwork.LeaveRoom();
        // Photon�̃T�[�o�[����ؒf����
        PhotonNetwork.Disconnect();
    }
}