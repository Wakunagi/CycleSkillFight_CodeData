using Program.InGame.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {

    /// <summary>
    /// 職業ごとのステータス保存先
    /// </summary>
    [CreateAssetMenu(fileName = "FighterDictionry", menuName = "ScriptableObjects/CreateFighterDictionary")]
    public class FighterDatas : ScriptableObject {

        /// <summary>
        /// 各職業のステータス
        /// </summary>
        [field: SerializeField] public List<FighterStatus>
            datas { private set; get; } = new List<FighterStatus>();

        /// <summary>
        /// 職業名からステータスを取得する関数
        /// </summary>
        /// <param name="name"> 職業名 </param>
        public FighterStatus GetSkillData(string name) {

            foreach (FighterStatus fighter in datas) {
                if (name == fighter.name) return fighter;
            }

            Debug.LogError("Skill : " + name + " is not set.");
            return null;
        }

        /// <summary>
        /// 職業名から見た目の画像を取得する関数
        /// </summary>
        /// <param name="name"> 職業名 </param>
        public Sprite GetImage(string name) {

            foreach (FighterStatus fighter in datas) {
                if (name == fighter.job) return fighter.img;
            }

            Debug.LogError("Job : " + name + " is not set.");
            return null;
        }
    }
}