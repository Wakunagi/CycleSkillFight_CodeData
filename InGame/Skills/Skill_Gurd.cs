using Program.GameSystem.Data;
using Program.InGame.Battle;
using Program.InGame.Skill;
using Unity.Mathematics;
using UnityEngine;

namespace Program.InGame.Skill {
    public class Skill_Gurd : SkillParent {

        [SerializeField] private int charge = 50;

        public override int GetCharge_SkillPower(SkillPower power) {
            return charge;
        }
        public override int GetCharge_SkillPower(AbilityType type) {
            return charge;
        }


        public override void Ability(FighterStatus me, FighterStatus enemy) {
            base.Ability(me, enemy);
            me.charge -= charge;
            me.isGurd = true;
        }

        public override void EndPhase(FighterStatus me, FighterStatus enemy) {
            me.isGurd = false;
        }
    }
}