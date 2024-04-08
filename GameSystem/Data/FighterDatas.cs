using Program.InGame.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {

    /// <summary>
    /// �E�Ƃ��Ƃ̃X�e�[�^�X�ۑ���
    /// </summary>
    [CreateAssetMenu(fileName = "FighterDictionry", menuName = "ScriptableObjects/CreateFighterDictionary")]
    public class FighterDatas : ScriptableObject {

        /// <summary>
        /// �e�E�Ƃ̃X�e�[�^�X
        /// </summary>
        [field: SerializeField] public List<FighterStatus>
            datas { private set; get; } = new List<FighterStatus>();

        /// <summary>
        /// �E�Ɩ�����X�e�[�^�X���擾����֐�
        /// </summary>
        /// <param name="name"> �E�Ɩ� </param>
        public FighterStatus GetSkillData(string name) {

            foreach (FighterStatus fighter in datas) {
                if (name == fighter.name) return fighter;
            }

            Debug.LogError("Skill : " + name + " is not set.");
            return null;
        }

        /// <summary>
        /// �E�Ɩ����猩���ڂ̉摜���擾����֐�
        /// </summary>
        /// <param name="name"> �E�Ɩ� </param>
        public Sprite GetImage(string name) {

            foreach (FighterStatus fighter in datas) {
                if (name == fighter.job) return fighter.img;
            }

            Debug.LogError("Job : " + name + " is not set.");
            return null;
        }
    }
}