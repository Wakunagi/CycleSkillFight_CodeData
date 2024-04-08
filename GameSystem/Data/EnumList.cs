namespace Program.GameSystem.Data {

    public class EnumList { }

    /// <summary>
    /// スキルパワーの種類
    /// </summary>
    public enum SkillPower {
        Low,
        Middle,
        High,
        End,
    }

    /// <summary>
    /// バトル時に使うスキルの種類
    /// </summary>
    public enum AbilityType {
        Low,
        Middle,
        High,
        Charge,
        End,
    }

    /// <summary>
    /// 入力の種類
    /// </summary>
    public enum InputPattern {
        Right, Left, Up, Down,
        RightArrow, LeftArrow, UpArrow, DownArrow,
        Home, Option, R, L,

        Decision, Cancel,

        End,
    }

    /// <summary>
    /// 各フェイズの種類
    /// </summary>
    public enum AnyPhase {
        PreparationPhase,
        SelectPhase,
        BattlePhase,
        EndPhase,

        End,
    }

    /// <summary>
    /// 勝敗の種類
    /// </summary>
    public enum WinLose {
        Win,Lose,End,
    }
}