using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerReloadUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        player.MyAnimator.SetBool("Reload", true);
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void StateUpdate()
    {
        if(!player.MyAnimator.GetBool("Reload"))
        {
            characterStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Normal);
        }
    }

}