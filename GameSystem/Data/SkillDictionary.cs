using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {

    /// <summary>
    /// 各スキルのデータの保存先
    /// </summary>
    [CreateAssetMenu(fileName = "SkillDictionary", menuName = "ScriptableObjects/CreateSkillDictionary")]
    public class SkillDictionary : ScriptableObject {

        /// <summary>
        /// スキルデータのリスト
        /// </summary>
        [field: SerializeField] public List<SkillData> datas { private set; get; } = new List<SkillData>();

        /// <summary>
        /// スキルのIDからスキル自体を取得
        /// </summary>
        /// <param name="name_id">スキルの名前型ID</param>
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