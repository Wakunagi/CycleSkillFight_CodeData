using Program.GameSystem.Data;
using Program.InGame.Battle;
using Program.InGame.Skill;
using System.Diagnostics;
using Unity.Mathematics;

namespace Program.InGame.Skill {
    public class Skill_NormalStab : AttackParents {

        int delta = 0;

        public override void Ability(FighterStatus me, FighterStatus enemy) {
            delta = math.abs(enemy.pos - me.pos);
            if (delta > 2) {
                me.pos += (enemy.pos - me.pos) / delta;
                delta = math.abs(enemy.pos - me.pos);
            }
            base.Ability(me, enemy);
        }

        public override void LowAbility(FighterStatus me, FighterStatus enemy) {
            if (delta <= lowReach) {
                enemy.life -= base.Damage(me, enemy, lowPower);
                if (delta > lowReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > lowReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }

        public override void MiddleAbility(FighterStatus me, FighterStatus enemy) {
            if (delta <= middleReach) {
                enemy.life -= base.Damage(me, enemy, middlePower);
                if (delta > middleReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > middleReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }

        public override void HighAbility(FighterStatus me, FighterStatus enemy) {
            if (delta <= highReach) {
                enemy.life -= base.Damage(me, enemy, highPower);
                if (delta > highReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > highReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }
    }
}