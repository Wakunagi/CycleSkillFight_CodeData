using Program.GameSystem.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Program.OutGame.UI {
    public class NW_SceneChangeManager : SceneChangeManager {

        [SerializeField] int player_max = 1;
        [SerializeField] NW_LogInManager login_manager;

        public override void SceneChanger(int decision) {
            decision_count += decision;

            if (decision_count == player_max) {
                login_manager.OnlineSceneChange(base.scene);
            }
            else {
                login_manager.DisConnected();
            }
        }
    }
}