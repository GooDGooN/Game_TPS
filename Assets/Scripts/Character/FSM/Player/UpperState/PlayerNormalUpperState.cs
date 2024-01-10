using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerNormalUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
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
                characterStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Reloading);
            }
        }
        if (player.FirePressing && player.MyState != GlobalEnums.CharacterState.Dash)
        {
            characterStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Firing);
        }
    }
}
