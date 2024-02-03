using CharacterNamespace;
using System;
using UnityEngine;

public class PlayerDashState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerDashState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;

        player.BlendPos = Vector2.up;
        player.MyAnimator.SetLayerWeight(1, 0);
        player.MyAnimator.SetBool("Dash", true);
        player.MyAnimator.SetBool("Reload", false);
        player.MyAnimator.Play("UpperIdle", 1, 0.0f);
        player.MoveSpeed = player.DefaultMoveSpeed * 1.5f;
    }
    public override void StateExit()
    {
        player.MyAnimator.SetLayerWeight(1, 1);
        player.MyAnimator.SetBool("Dash", false);
    }

    public override void StateFixedUpdate()
    {
        player.TempMoveDirection = player.PlayerBody.transform.forward;
        Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, GlobalVarStorage.SolidLayer);
        if (playerRay.distance > player.CapsuleColliderHeight + 0.05f)
        {
            characterStateController.ChangeState(CharacterState.MidAir);
        }
        if (player.CheckJump)
        {
            player.MyRigidbody.AddForce(Vector3.up * player.JumpPower);
            characterStateController.ChangeState(CharacterState.MidAir);
        }
    }

    public override void StateUpdate()
    {
        if(player.IsStaminaRecharge)
        {
            if(player.ForwardPressing)
            {
                characterStateController.ChangeState(CharacterState.Move);
            }
            else
            {
                characterStateController.ChangeState(CharacterState.Idle);
            }
        }
        if (player.ForwardPressing && !player.DashPressing)
        {
            characterStateController.ChangeState(CharacterState.Move);
        }
        else if(!player.ForwardPressing)
        {
            characterStateController.ChangeState(CharacterState.Idle);
        }

        if (player.JumpPressed)
        {
            player.CheckJump = true;
        }
    }
}
