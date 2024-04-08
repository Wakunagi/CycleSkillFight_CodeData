namespace Program.GameSystem.Data {

    public class EnumList { }

    /// <summary>
    /// �X�L���p���[�̎��
    /// </summary>
    public enum SkillPower {
        Low,
        Middle,
        High,
        End,
    }

    /// <summary>
    /// �o�g�����Ɏg���X�L���̎��
    /// </summary>
    public enum AbilityType {
        Low,
        Middle,
        High,
        Charge,
        End,
    }

    /// <summary>
    /// ���͂̎��
    /// </summary>
    public enum InputPattern {
        Right, Left, Up, Down,
        RightArrow, LeftArrow, UpArrow, DownArrow,
        Home, Option, R, L,

        Decision, Cancel,

        End,
    }

    /// <summary>
    /// �e�t�F�C�Y�̎��
    /// </summary>
    public enum AnyPhase {
        PreparationPhase,
        SelectPhase,
        BattlePhase,
        EndPhase,

        End,
    }

    /// <summary>
    /// ���s�̎��
    /// </summary>
    public enum WinLose {
        Win,Lose,End,
    }
}