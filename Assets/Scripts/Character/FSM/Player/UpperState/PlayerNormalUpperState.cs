using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalUpperState : PlayerBaseFSM
{
    public PlayerNormalUpperState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void StateUpdate()
    {
        if (player.ReloadPressed)
        {
            if (player.MyState != GlobalEnums.CharacterState.Dash)
            {
                playerStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Reloading);
            }
        }
        if (player.FirePressing && player.MyState != GlobalEnums.CharacterState.Dash)
        {
            playerStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Firing);
        }
    }
}
