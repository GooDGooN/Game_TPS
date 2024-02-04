using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    private PlayerRifleControl playerRifle;
    public PlayerNormalUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        playerRifle = player.PlayerRifle;
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
            if (player.MyState != CharacterState.Dash && playerRifle.CurrentMagazineCapacity != playerRifle.MaxImumMagazineCapacity)
            {
                characterStateController.ChangeState(CharacterUpperState.Reloading);
            }
        }
        if (player.FirePressing && player.MyState != CharacterState.Dash)
        {
            characterStateController.ChangeState(CharacterUpperState.Firing);
        }
    }
}
