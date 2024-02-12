using Program.GameSystem.Data;
using Program.InGame.Battle;
using Program.InGame.Skill;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace Program.InGame.Skill {
    public class Skill_BackStep : SkillParent {
        public override void Ability(FighterStatus me, FighterStatus enemy) {
            base.Ability(me, enemy);
            int delta = math.abs(enemy.pos - me.pos);
            if (delta > 2) return;

            me.pos -= (enemy.pos - me.pos) / delta;
        }
    }
}