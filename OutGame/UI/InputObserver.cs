using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Program.GameSystem;
using Program.GameSystem.Data;

namespace Program.OutGame.UI {
    public class InputObserver : MonoBehaviour {

        [SerializeField] private float left_h, left_v, right_h, right_v;
        [SerializeField] private InputManager rightManager, leftManager;

        float old_lh = 0, old_lv = 0, old_rh = 0, old_rv = 0;

        void Update() {
            //入力用真偽値

            //左
            left_h = Input.GetAxisRaw("Horizontal1");
            left_v = Input.GetAxisRaw("Vertical1");
            bool isLeftInput_right = (left_v > 0 && left_v != old_lv) || Input.GetKeyDown(KeyCode.D);
            bool isLeftInput_left = (left_v < 0 && left_v != old_lv) || Input.GetKeyDown(KeyCode.A);
            bool isLeftInput_up = (left_h < 0 && left_h != old_lh) || Input.GetKeyDown(KeyCode.W);
            bool isLeftInput_down = (left_h > 0 && left_h != old_lh) || Input.GetKeyDown(KeyCode.S);
            bool isDecisionInput_left = Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.LeftShift);
            bool isCancelInput_left = Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.LeftControl);
            bool isLeftInput_rightArrow = Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.D);
            bool isLeftInput_leftArrow = Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.A);
            bool isLeftInput_upArrow = Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.W);
            bool isLeftInput_downArrow = Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.S);

            //右
            right_h = Input.GetAxisRaw("Horizontal2");
            right_v = Input.GetAxisRaw("Vertical2");
            bool isRightInput_right = (right_v < 0 && right_v != old_rv) || Input.GetKeyDown(KeyCode.RightArrow);
            bool isRightInput_left = (right_v > 0 && right_v != old_rv) || Input.GetKeyDown(KeyCode.LeftArrow);
            bool isRightInput_up = (right_h > 0 && right_h != old_rh) || Input.GetKeyDown(KeyCode.UpArrow);
            bool isRightInput_down = (right_h < 0 && right_h != old_rh) || Input.GetKeyDown(KeyCode.DownArrow);
            bool isDecisionInput_right = Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKeyDown(KeyCode.RightShift)||Input.GetKeyDown(KeyCode.Return);
            bool isCancelInput_right = Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.RightControl);
            bool isRightInput_rightArrow = Input.GetKeyDown(KeyCode.Joystick2Button1) || Input.GetKeyDown(KeyCode.RightArrow);
            bool isRightInput_leftArrow = Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKeyDown(KeyCode.LeftArrow);
            bool isRightInput_upArrow = Input.GetKeyDown(KeyCode.Joystick2Button3) || Input.GetKeyDown(KeyCode.UpArrow);
            bool isRightInput_downArrow = Input.GetKeyDown(KeyCode.Joystick2Button0) || Input.GetKeyDown(KeyCode.DownArrow);

            if (isLeftInput_right) leftManager.InputAnyButton(InputPattern.Right);
            if (isLeftInput_left) leftManager.InputAnyButton(InputPattern.Left);
            if (isLeftInput_up) leftManager.InputAnyButton(InputPattern.Up);
            if (isLeftInput_down) leftManager.InputAnyButton(InputPattern.Down);
            if (isDecisionInput_left) leftManager.InputAnyButton(InputPattern.Decision);
            if(isCancelInput_left)leftManager.InputAnyButton(InputPattern.Cancel);
            if (isLeftInput_rightArrow) leftManager.InputAnyButton(InputPattern.RightArrow);
            if (isLeftInput_leftArrow) leftManager.InputAnyButton(InputPattern.LeftArrow);
            if (isLeftInput_upArrow) leftManager.InputAnyButton(InputPattern.UpArrow);
            if (isLeftInput_downArrow) leftManager.InputAnyButton(InputPattern.DownArrow);

            if (isRightInput_right) rightManager.InputAnyButton(InputPattern.Right);
            if (isRightInput_left) rightManager.InputAnyButton(InputPattern.Left);
            if (isRightInput_up) rightManager.InputAnyButton(InputPattern.Up);
            if (isRightInput_down) rightManager.InputAnyButton(InputPattern.Down);
            if (isDecisionInput_right) rightManager.InputAnyButton(InputPattern.Decision);
            if (isCancelInput_right) rightManager.InputAnyButton(InputPattern.Cancel);
            if (isRightInput_rightArrow) rightManager.InputAnyButton(InputPattern.RightArrow);
            if (isRightInput_leftArrow) rightManager.InputAnyButton(InputPattern.LeftArrow);
            if (isRightInput_upArrow) rightManager.InputAnyButton(InputPattern.UpArrow);
            if (isRightInput_downArrow) rightManager.InputAnyButton(InputPattern.DownArrow);

            old_lh = left_h;
            old_lv = left_v;
            old_rh = right_h;
            old_rv = right_v;
        }

    }
}