using CharacterNamespace;
using UnityEngine;
using static GlobalEnums;

public class PlayerDashState : PlayerBaseFSM
{
    public PlayerDashState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
        playerControl.BlendPos = Vector2.up;
        playerControl.MyAnimator.SetLayerWeight(1, 0);
        playerControl.MyAnimator.SetBool("Dash", true);
        playerControl.MyAnimator.SetBool("Reload", false);
        playerControl.MyAnimator.Play("UpperIdle", 1, 0.0f);
        playerControl.MoveSpeed = 350.0f;
    }
    public override void StateExit()
    {
        playerControl.MyAnimator.SetLayerWeight(1, 1);
        playerControl.MyAnimator.SetBool("Dash", false);
    }

    public override void StateFixedUpdate()
    {
        playerControl.TempMoveDirection = playerControl.PlayerBody.transform.forward;
        Physics.Raycast(playerControl.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, playerControl.SolidLayer);
        if (playerRay.distance > playerControl.ColliderHeight + 0.2f)
        {
            playerStateController.ChangeState(CharacterState.MidAir);
        }
        if (playerControl.CheckJump)
        {
            playerControl.MyRigidbody.AddForce(Vector3.up * playerControl.JumpPower);
            playerStateController.ChangeState(CharacterState.MidAir);
        }
    }

    public override void StateUpdate()
    {
        if (playerControl.ForwardPressing && !playerControl.DashPressing)
        {
            playerStateController.ChangeState(CharacterState.Move);
        }
        else if(!playerControl.ForwardPressing)
        {
            playerStateController.ChangeState(CharacterState.Idle);
        }
        if (playerControl.JumpPressed)
        {
            playerControl.CheckJump = true;
        }
    }

}
