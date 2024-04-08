using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {

    /// <summary>
    /// �e�X�L���̃f�[�^�̕ۑ���
    /// </summary>
    [CreateAssetMenu(fileName = "SkillDictionary", menuName = "ScriptableObjects/CreateSkillDictionary")]
    public class SkillDictionary : ScriptableObject {

        /// <summary>
        /// �X�L���f�[�^�̃��X�g
        /// </summary>
        [field: SerializeField] public List<SkillData> datas { private set; get; } = new List<SkillData>();

        /// <summary>
        /// �X�L����ID����X�L�����̂��擾
        /// </summary>
        /// <param name="name_id">�X�L���̖��O�^ID</param>
        /// <returns></returns>
        public SkillData GetSkillData(string name_id) {

            foreach (SkillData skillData in datas) {
                if (name_id == skillData.name_id) return skillData;
            }

            Debug.LogError("Skill : " + name_id + " is not set.");
            return null;
        }
    }
}