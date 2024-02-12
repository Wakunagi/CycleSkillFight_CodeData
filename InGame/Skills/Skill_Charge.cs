using Program.InGame.Battle;
using Program.GameSystem.Data;
using UnityEngine.UI;
using UnityEngine;

namespace Program.InGame.Skill {
    public class Skill_Charge : SkillParent {
        [SerializeField]  string name_txt;

      [SerializeField, TextAreaAttribute(1, 10)] string explanation_txt;

        public override void Ability(FighterStatus me, FighterStatus enemy) {
            base.Ability(me, enemy);
            me.charge += MyConst.CHARGE_PERCENT;
            if (me.charge > MyConst.CHARGE_MAX) me.charge = MyConst.CHARGE_MAX;

            me.def *= 0.8f;
        }

        public override void EndPhase(FighterStatus me, FighterStatus enemy) {
            base.EndPhase(me, enemy);

           me.def /= 0.8f;
        }

        public override void SetExplanation(Text name_text, Text explanation_text) {
            name_text.text = name_txt;
            explanation_text.text = explanation_txt;
        }
    }
}
