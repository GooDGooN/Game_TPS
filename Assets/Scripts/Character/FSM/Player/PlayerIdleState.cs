
using CharacterNamespace;
using Unity.Physics;
using UnityEngine;
using static GlobalEnums;

public class PlayerIdleState : PlayerBaseFSM
{
    public PlayerIdleState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
        playerControl.TempMoveDirection = Vector3.zero;
        playerControl.BlendPos = Vector2.zero;
    }
    public override void StateExit()
    {
    }
    public override void StateUpdate()
    {
        if (playerControl.RightPressing ^ playerControl.LeftPressing || playerControl.ForwardPressing ^ playerControl.BackPressing)
        {
            playerStateController.ChangeState(CharacterState.Move);
        }
        if (playerControl.JumpPressed)
        {
            playerControl.CheckJump = true;
        }
        playerControl.MyRigidbody.velocity = new Vector3(playerControl.MyRigidbody.velocity.x, 0.0f, playerControl.MyRigidbody.velocity.z);
    }
    public override void StateFixedUpdate() 
    {
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
}
