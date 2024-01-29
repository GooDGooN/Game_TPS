using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerFireUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        player.MyAnimator.SetBool("Fire", true);
    }

    public override void StateExit()
    {
        player.MyAnimator.SetBool("Fire", false);
    }

    public override void StateFixedUpdate()
    {
        if (player.ReloadPressed)
        {
            characterStateController.ChangeState(CharacterUpperState.Reloading);
        }
        if (!player.FirePressing)
        {
            characterStateController.ChangeState(CharacterUpperState.Normal);
        }
    }

    public override void StateUpdate()
    {
        if (player.FireDelay <= 0.0f)
        {
            player.FireDelay = player.FireRate;
            player.MyAnimator.Play("UpperFire", 1);
            player.HitScanBullet.SetActive(true);
            player.HitScanBullet.GetComponent<HitScanBullet>().ActiveBullet(player.BulletHitPoint);
        }

        if(player.MyAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f)
        {
            player.MyAnimator.Play("UpperFireDelayState", 1);
        }
    }
}
