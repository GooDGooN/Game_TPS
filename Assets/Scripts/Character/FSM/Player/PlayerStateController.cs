using CharacterNamespace;
using static GlobalEnums;

public class PlayerStateController
{
    private PlayerControl playerControl;
    private PlayerBaseFSM currentState;
    public PlayerBaseFSM CurrentState { get => currentState; }
    public CharacterState LastState { get => lastState; }
    private CharacterState lastState;

    private PlayerBaseFSM currentUpperState;
    public PlayerBaseFSM CurrentUpperState { get => currentUpperState; }
    public CharacterUpperState LastUpperState { get => lastUpperState; }
    private CharacterUpperState lastUpperState;


    public PlayerStateController(PlayerControl target)
    {
        playerControl = target;
    }

    public void ChangeState(CharacterState state)
    {
        if(lastState != playerControl.MyState)
        {
            lastState = playerControl.MyState;
        }
        playerControl.MyState = state;
        currentState?.StateExit();
        switch(state)
        {
            case CharacterState.Idle:
                currentState = new PlayerIdleState(playerControl, this);
                break;
            case CharacterState.Move:
                currentState = new PlayerMoveState(playerControl, this);
                break;
            case CharacterState.MidAir:
                currentState = new PlayerMidAirState(playerControl, this);
                break;
            case CharacterState.Dash:
                currentState = new PlayerDashState(playerControl, this);
                break;
        }
        currentState.StateEnter();
    }
    public void ChangeUpperState(CharacterUpperState state)
    {
        if(lastUpperState != playerControl.MyUpperState)
        {
            lastUpperState = playerControl.MyUpperState;
        }
        playerControl.MyUpperState = state;
        currentUpperState?.StateExit();
        switch (state)
        {
            case CharacterUpperState.Normal:
                currentUpperState = new PlayerNormalUpperState(playerControl, this);
                break;
            case CharacterUpperState.Firing:
                currentUpperState = new PlayerFireUpperState(playerControl, this);
                break;
            case CharacterUpperState.Reloading:
                currentUpperState = new PlayerReloadUpperState(playerControl, this);
                break;
        }
        currentUpperState.StateEnter();
    }
}
