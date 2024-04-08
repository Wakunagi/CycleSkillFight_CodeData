using Program.InGame.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {

    /// <summary>
    /// �퓬�V�[���ɏ���n�����߂̃N���X
    /// </summary>
    [CreateAssetMenu(fileName = "FighterData", menuName = "ScriptableObjects/CreateFighterData")]
    public class FighterSkillData : ScriptableObject {

        //�X�e�[�^�X
        [field: SerializeField]
        public FighterStatus status { private set; get; } = new FighterStatus();
        public void SetFighterStatus(FighterStatus s) { status = s; }

        //�����X�L��
        [field: SerializeField] public string[] skills { private set; get; } = new string[6];
        public void SetSkill(int num, string skill) { skills[num] = skill; }

        //�ʕϐ��Ƃ��ăX�e�[�^�X���Z�b�g����
        public void SetStatus(FighterStatus target, FighterStatus from) {
            string name = from.name;
            float life = from.life, atk = from.atk, def = from.def, spd = from.spd;
            Sprite img = from.img;
            int pos = 0;
            int charge = 0;
            bool isGurd = false;

            target.name = name;
            target.life = life;
            target.atk = atk;
            target.def = def;
            target.spd = spd;
            target.img = img;
            target.pos = pos;
            target.charge = charge;
            target.isGurd = isGurd;
        }
    }

}