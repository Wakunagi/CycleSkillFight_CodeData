namespace Program.GameSystem.Data {

    public class EnumList { }

    //スキルパワー
    public enum SkillPower {
        Low,
        Middle,
        High,
        End,
    }

    public enum AbilityType {
        Low,
        Middle,
        High,
        Charge,
        End,
    }

    //入力パターン
    public enum InputPattern {
        Right, Left, Up, Down,
        RightArrow, LeftArrow, UpArrow, DownArrow,
        Home, Option, R, L,

        Decision, Cancel,

        End,
    }

    //各フェイズ
    public enum AnyPhase {
        PreparationPhase,
        SelectPhase,
        BattlePhase,
        EndPhase,

        End,
    }

    public enum WinLose {
        Win,Lose,End,
    }
}