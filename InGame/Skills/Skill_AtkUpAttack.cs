using Program.GameSystem.Data;
using Program.InGame.Battle;
using Program.InGame.Skill;
using Unity.Mathematics;
using UnityEngine;

namespace Program.InGame.Skill {
    public class Skill_AtkUpAttack : AttackParents {

        [SerializeField, HeaderAttribute("è„è∏íl")] private float low_atk_upper, middle_atk_upper, high_atk_upper;

        public override void Ability(FighterStatus me, FighterStatus enemy) {
            base.Ability(me, enemy);
        }

        public override void LowAbility(FighterStatus me, FighterStatus enemy) {
            int delta = math.abs(enemy.pos - me.pos);
            if (delta <= lowReach) {
                enemy.life -= base.Damage(me, enemy, lowPower);
                me.atk *= low_atk_upper;
                if (delta > lowReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > lowReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }

        public override void MiddleAbility(FighterStatus me, FighterStatus enemy) {
            int delta = math.abs(enemy.pos - me.pos);
            if (delta <= middleReach) {
                enemy.life -= base.Damage(me, enemy, middlePower);
                me.atk *= middle_atk_upper;
                if (delta > middleReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > middleReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }

        public override void HighAbility(FighterStatus me, FighterStatus enemy) {
            int delta = math.abs(enemy.pos - me.pos);
            if (delta <= highReach) {
                enemy.life -= base.Damage(me, enemy, highPower);
                me.atk *= high_atk_upper;
                if (delta > highReach) me.pos += (enemy.pos - me.pos) / delta;
            }
            else {
                if (delta > highReach) me.pos += (enemy.pos - me.pos) / delta;
            }
        }
    }
}