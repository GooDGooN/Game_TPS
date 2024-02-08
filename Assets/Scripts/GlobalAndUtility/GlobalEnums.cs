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
    Interact,
}

public enum CharacterState
{
    Idle,
    Move,
    Dash,
    MidAir,
    Attack,
    Death,
}

public enum CharacterUpperState
{
    Normal,
    Reloading,
    Firing,
}
public enum BulletUserType
{
    None,
    Player,
    Enemy,
}

public enum EnemyType
{
    SlimeRabbit,
    Bee,
    Mushroom,
    Bat,
    Mummy,
}