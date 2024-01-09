using CharacterNamespace;
using UnityEngine;
using static GlobalEnums;

public class PlayerDashState : PlayerBaseFSM
{
    public PlayerDashState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
        player.BlendPos = Vector2.up;
        player.MyAnimator.SetLayerWeight(1, 0);
        player.MyAnimator.SetBool("Dash", true);
        player.MyAnimator.SetBool("Reload", false);
        player.MyAnimator.Play("UpperIdle", 1, 0.0f);
        player.MoveSpeed = 350.0f;
    }
    public override void StateExit()
    {
        player.MyAnimator.SetLayerWeight(1, 1);
        player.MyAnimator.SetBool("Dash", false);
    }

    public override void StateFixedUpdate()
    {
        player.TempMoveDirection = player.PlayerBody.transform.forward;
        Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, player.SolidLayer);
        if (playerRay.distance > player.ColliderHeight + 0.2f)
        {
            playerStateController.ChangeState(CharacterState.MidAir);
        }
        if (player.CheckJump)
        {
            player.MyRigidbody.AddForce(Vector3.up * player.JumpPower);
            playerStateController.ChangeState(CharacterState.MidAir);
        }
    }

    public override void StateUpdate()
    {
        if (player.ForwardPressing && !player.DashPressing)
        {
            playerStateController.ChangeState(CharacterState.Move);
        }
        else if(!player.ForwardPressing)
        {
            playerStateController.ChangeState(CharacterState.Idle);
        }
        if (player.JumpPressed)
        {
            player.CheckJump = true;
        }
    }

}
