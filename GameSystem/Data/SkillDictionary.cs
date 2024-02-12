using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {
    [CreateAssetMenu(fileName = "SkillDictionary", menuName = "ScriptableObjects/CreateSkillDictionary")]
    public class SkillDictionary : ScriptableObject {

        [field: SerializeField] public List<SkillData> datas { private set; get; } = new List<SkillData>();

        public SkillData GetSkillData(string name_id) {

            foreach (SkillData skillData in datas) {
                if (name_id == skillData.name_id) return skillData;
            }

            Debug.LogError("Skill : " + name_id + " is not set.");
            return null;
        }
    }
}