using Program.InGame.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Program.GameSystem.Data {
    [CreateAssetMenu(fileName = "FighterDictionry", menuName = "ScriptableObjects/CreateFighterDictionary")]
    public class FighterDatas : ScriptableObject {
        [field: SerializeField] public List<FighterStatus> datas { private set; get; } = new List<FighterStatus>();

        public FighterStatus GetSkillData(string name) {

            foreach (FighterStatus fighter in datas) {
                if (name == fighter.name) return fighter;
            }

            Debug.LogError("Skill : " + name + " is not set.");
            return null;
        }

        public Sprite GetImage(string name) {
            foreach (FighterStatus fighter in datas) {
                if (name == fighter.job) return fighter.img;
            }

            Debug.LogError("Job : " + name + " is not set.");
            return null;
        }
    }
}