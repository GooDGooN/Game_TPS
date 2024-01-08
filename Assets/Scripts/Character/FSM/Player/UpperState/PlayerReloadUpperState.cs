using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadUpperState : PlayerBaseFSM
{
    public PlayerReloadUpperState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
        playerControl.MyAnimator.SetBool("Reload", true);
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void StateUpdate()
    {
        if(!playerControl.MyAnimator.GetBool("Reload"))
        {
            playerStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Normal);
        }
    }

}