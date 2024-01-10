using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalEnums;

public class PlayerState
{
    public static CharacterBaseFSM ReturnState(CharacterState state, CharacterStateController stateController)
    {
        var player = stateController.TargetCharacter as PlayerControl;
        switch (state)
        {
            case CharacterState.Idle:
                return new PlayerIdleState(stateController, player);
            case CharacterState.MidAir:
                return new PlayerMidAirState(stateController, player);
            case CharacterState.Move:
                return new PlayerMoveState(stateController, player);
            case CharacterState.Dash:
                return new PlayerDashState(stateController, player);
            default: return null;
        }
    }
    public static CharacterBaseFSM ReturnUpperState(CharacterUpperState state, CharacterStateController stateController)
    {
        var player = stateController.TargetCharacter as PlayerControl;
        switch (state)
        {
            case CharacterUpperState.Normal:
                return new PlayerNormalUpperState(stateController, player);
            case CharacterUpperState.Firing:
                return new PlayerFireUpperState(stateController, player);
            case CharacterUpperState.Reloading:
                return new PlayerReloadUpperState(stateController, player);
            default: return null;
        }
    }
}
