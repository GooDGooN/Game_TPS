using CharacterNamespace;
using System;
using static GlobalEnums;

public class CharacterStateController
{
    public CharacterProperty TargetCharacter { get => targetCharacter; }
    private CharacterProperty targetCharacter;
    private CharacterBaseFSM currentState;
    public CharacterBaseFSM CurrentState { get => currentState; }
    public CharacterState LastState { get => lastState; }
    private CharacterState lastState;

    private CharacterBaseFSM currentUpperState;
    public CharacterBaseFSM CurrentUpperState { get => currentUpperState; }
    public CharacterUpperState LastUpperState { get => lastUpperState; }
    private CharacterUpperState lastUpperState;


    public CharacterStateController(CharacterProperty targetCharacter)
    {
        this.targetCharacter = targetCharacter;
    }

    public void ChangeState(CharacterState state)
    {
        if(lastState != targetCharacter.MyState)
        {
            lastState = targetCharacter.MyState;
        }
        currentState?.StateExit();

        if(targetCharacter is PlayerControl)
        {
            currentState = PlayerState.ReturnState(state, this);
        }

        targetCharacter.MyState = state;
        currentState.StateEnter();
    }

    public void ChangeUpperState(CharacterUpperState state)
    {
        if(lastUpperState != targetCharacter.MyUpperState)
        {
            lastUpperState = targetCharacter.MyUpperState;
        }

        currentUpperState?.StateExit();

        if(targetCharacter is PlayerControl)
        {
            currentUpperState = PlayerState.ReturnUpperState(state, this);
        }
        targetCharacter.MyUpperState = state;
        currentUpperState.StateEnter();
    }
}
