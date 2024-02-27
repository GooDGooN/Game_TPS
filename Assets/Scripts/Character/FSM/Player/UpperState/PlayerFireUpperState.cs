using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireUpperState : CharacterBaseFSM
{
    private PlayerControl player;
    private PlayerRifleControl playerRifle;
    public PlayerFireUpperState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;
        playerRifle = player.PlayerRifle;
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
        if (!player.FirePressing || player.MyState == CharacterState.Dash)
        {
            characterStateController.ChangeState(CharacterUpperState.Normal);
        }
    }

    public override void StateUpdate()
    {
        if (player.AttackDelay <= 0.0f)
        {
            if(player.PlayerRifle.CurrentMagazineCapacity > 0)
            {
                player.AttackDelay = player.AtkSpeed;
                player.playerRifleAudio.PlaySound(SoundType.RifleFire);
                player.MyAnimator.Play("UpperFire", 1);
                playerRifle.BulletFire(player.BulletHitPoint, player.AtkDamage);
            }
            else
            {
                player.AttackDelay = player.AtkSpeed;
                player.playerRifleAudio.PlaySound(SoundType.RifleMagDry);
            }
        }

        if(player.MyAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.0f)
        {
            player.MyAnimator.Play("UpperFireDelayState", 1);
        }
    }
}
