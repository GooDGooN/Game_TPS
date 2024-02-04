using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    private PlayerRifleControl playerRifle;
    public PlayerReloadUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        playerRifle = player.PlayerRifle;
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
        if(player.ReloadComplete)
        {
            player.PlayerRifle.ReloadMagazine();
            player.ReloadComplete = false;
        }
        if(!player.MyAnimator.GetBool("Reload"))
        {
            characterStateController.ChangeState(CharacterUpperState.Normal);
        }
    }

}