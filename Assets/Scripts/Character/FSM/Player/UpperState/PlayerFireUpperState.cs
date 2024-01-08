using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireUpperState : PlayerBaseFSM
{
    public PlayerFireUpperState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    private float delay;
    public override void StateEnter()
    {
        playerControl.MyAnimator.SetBool("Fire", true);
        delay = 0.0f;
    }

    public override void StateExit()
    {
        playerControl.MyAnimator.SetBool("Fire", false);
    }

    public override void StateFixedUpdate()
    {
        if (playerControl.ReloadPressed)
        {
            playerStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Reloading);
        }
        if (!playerControl.FirePressing)
        {
            playerStateController.ChangeUpperState(GlobalEnums.CharacterUpperState.Normal);
        }
    }

    public override void StateUpdate()
    {
        delay -= 0.1f;
        if (delay <= 0.0f)
        {
            delay = playerControl.FireRate;
            playerControl.MyAnimator.Play("UpperFire", 1);
            BulletContainer.Instance.BulletActive(playerControl.BulletHitPoint, GlobalEnums.BulletPoolType.Player);
        }

        if(playerControl.MyAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f)
        {
            playerControl.MyAnimator.Play("UpperFireDelayState", 1);
        }
    }
}
