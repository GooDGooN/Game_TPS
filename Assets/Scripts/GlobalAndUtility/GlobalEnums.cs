
public class GlobalEnums
{
    public enum KeyInputs
    {
        MoveRight,
        MoveLeft,
        MoveFoward, 
        MoveBack,
        FreeView,
        Dash,
        Jump,
        Fire,
        ZoomIn,
        Reload,
    }

    public enum CharacterState
    {
        Idle,
        Move,
        Dash,
        MidAir,
    }
    public enum CharacterUpperState
    {
        Normal,
        Reloading,
        Firing,
    }
    public enum BulletPoolType
    {
        None,
        Player,
        Enemy,
    }
}