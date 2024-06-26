using UnityEngine;

namespace Program.GameSystem.Data {
    /// <summary>
    /// 各スキルのデータ
    /// </summary>
    [System.Serializable]
    public class SkillData {
        public SkillPower skillPower;
        public string name_id;
        public string name;
        [TextAreaAttribute(1, 10)]
        public string explanation;
        public GameObject prefab;
        public Sprite image;
    }
}