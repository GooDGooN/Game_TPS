using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerFireUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    private float delay;
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        player.MyAnimator.SetBool("Fire", true);
        delay = 0.0f;
    }

    public override void StateExit()
    {
        player.MyAnimator.SetBool("Fire", false);
    }

    public override void StateFixedUpdate()
    {
        if (player.ReloadPressed)
        {
            characterStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Reloading);
        }
        if (!player.FirePressing)
        {
            characterStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Normal);
        }
    }

    public override void StateUpdate()
    {
        delay -= 0.1f;
        if (delay <= 0.0f)
        {
            delay = player.FireRate;
            player.MyAnimator.Play("UpperFire", 1);
            BulletContainer.Instance.BulletActive(player.BulletHitPoint, GlobalEnums.BulletPoolType.Player);
        }

        if(player.MyAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f)
        {
            player.MyAnimator.Play("UpperFireDelayState", 1);
        }
    }
}
