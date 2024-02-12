using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Program.GameSystem.Data;
using Program.InGame.Battle;

namespace Program.InGame.Skill {
    public class SkillParent : MonoBehaviour {
        SkillData mySkillData = null;
        public void SetSkillData(SkillData data) { mySkillData = data; }

        [SerializeField]protected SkillPower skillPower;
        [field: SerializeField] public int priority { private set; get; } = 0;

        [SerializeField]Sprite myImg = null;
        Image animation_img = null;
        public void SetSprite(Sprite img) { myImg = img; }
        public void SetImage(Image img) { animation_img = img; }

       protected BattleManager battleManager;
        public void SetBattleManager(BattleManager bm) { battleManager = bm; }

        public virtual int GetCharge_SkillPower(SkillPower power) {
            return 0;
        }
        public virtual int GetCharge_SkillPower(AbilityType type) {
            return 0;
        }

        public void SetSkillPower(SkillPower sp) { skillPower = sp; }

        public virtual void Ability(FighterStatus me, FighterStatus enemy) {
            StartCoroutine(Animation());
        }

        public virtual void SetExplanation(Text name_text,Text explanation_text) {
            name_text.text = mySkillData.name;
            explanation_text.text = mySkillData.explanation;
        }

        public virtual IEnumerator Animation() {
            float i = 1f;
            animation_img.sprite = myImg;
            while (true) {

                animation_img.color = new Color(1, 1, 1, i);
                if (i < 0) break;

                i -= Time.deltaTime*0.8f;
                yield return null;
            }
            battleManager.CallBack();
        }

        public virtual void EndPhase(FighterStatus me, FighterStatus enemy) { }

    }
}