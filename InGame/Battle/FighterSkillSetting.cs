using Program.InGame.Skill;

namespace Program.InGame.Battle {

    [System.Serializable]

    public class FighterSkillSetting {

        public FighterStatus status = null;
        public SkillParent skill = null;
        public int my_num = 0, enemy_num = 0;
        public int order = 0;

    }
}