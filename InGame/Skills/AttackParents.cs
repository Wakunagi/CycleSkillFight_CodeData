using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Program.GameSystem.Data;
using Program.InGame.Battle;

namespace Program.InGame.Skill {
    public class AttackParents : SkillParent {

        [field: SerializeField, HeaderAttribute("必要チャージ量")] public int lowCharge { private set; get; } = 0;
        [field: SerializeField] public int middleCharge { private set; get; } = 50;
        [field: SerializeField] public int highCharge { private set; get; } = 100;

        [SerializeField, HeaderAttribute("リーチ")] protected int lowReach = 2;
        [SerializeField] protected int middleReach = 2;
        [SerializeField] protected int highReach = 2;

        [SerializeField, HeaderAttribute("威力")] protected int lowPower = 1;
        [SerializeField] protected int middlePower = 2;
        [SerializeField] protected int highPower = 3;


        public override int GetCharge_SkillPower(SkillPower power) {
            if (power == SkillPower.Low) return lowCharge;
            else
            if (power == SkillPower.Middle) return middleCharge;
            else
            if (power == SkillPower.High) return highCharge;
            else return -1;
        }
        public override int GetCharge_SkillPower(AbilityType type) {
            if (type == AbilityType.Low) return lowCharge;
            else
            if (type == AbilityType.Middle) return middleCharge;
            else
            if (type == AbilityType.High) return highCharge;
            else return -1;
        }

        public override void Ability(FighterStatus me, FighterStatus enemy) {
            base.Ability(me, enemy);
            switch (skillPower) {
                case SkillPower.Low: me.charge -= lowCharge; break;
                case SkillPower.Middle: me.charge -= middleCharge; break;
                case SkillPower.High: me.charge -= highCharge; break;
                default: break;
            }

            if (enemy.isGurd) {
                return;
            }

            base.Ability(me, enemy);
            Debug.Log("1 Me" + me.pos + "  :  Enemy" + me.pos);
            switch (skillPower) {
                case SkillPower.Low: LowAbility(me, enemy); break;
                case SkillPower.Middle: MiddleAbility(me, enemy); break;
                case SkillPower.High: HighAbility(me, enemy); break;
                default: break;
            }
            Debug.Log("2 Me" + me.pos + "  :  Enemy" + me.pos);
        }

        public virtual float Damage(FighterStatus me, FighterStatus enemy, int power) {
            enemy.isDamaged = true;
            return me.atk / enemy.def * power / enemy.def  * 10;
        }

        public virtual void LowAbility(FighterStatus me, FighterStatus enemy) {

        }

        public virtual void MiddleAbility(FighterStatus me, FighterStatus enemy) {

        }

        public virtual void HighAbility(FighterStatus me, FighterStatus enemy) {

        }

        public override void EndPhase(FighterStatus me, FighterStatus enemy) { }

    }
}