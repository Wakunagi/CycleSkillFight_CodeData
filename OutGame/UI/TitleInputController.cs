using UnityEngine;
using UnityEngine.UI;
using Program.GameSystem;
using Unity.Mathematics;
using UnityEngine.Purchasing.MiniJSON;

namespace Program.OutGame.UI {
    public class TitleInputController : InputManager {

        [SerializeField] private Text prease_input_text;

        [SerializeField] private string before, after;

        bool isOn = false;

        [SerializeField] float t = 0;
        [SerializeField] float alfa = 0;

        private void Start() {
            prease_input_text.text = before;
        }

        private void Update() {
            t += Time.deltaTime;
            if (t > math.PI) t = 0;

            alfa = math.sin(t) * 0.9f + 0.1f;

            prease_input_text.color = new Color(0, 0, 0, alfa);
        }

        protected override void InputDecision() {
            if (isOn) return;
            isOn = true;
            prease_input_text.text = after;
            sceneChangeManager.SceneChanger(1);
            base.InputDecision();
        }

    }
}