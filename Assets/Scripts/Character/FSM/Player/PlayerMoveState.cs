using CharacterNamespace;
using System;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering;
using static GlobalEnums;

public class PlayerMoveState : PlayerBaseFSM
{
    public PlayerMoveState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }
    public override void StateEnter()
    {
        playerControl.MoveSpeed = 200.0f;
    }

    public override void StateExit()
    {
        /*playerctr.TempMoveDirection = Vector3.zero;
        playerctr.BlendPos = Vector2.zero;*/
    }

    public override void StateUpdate()
    {
        playerControl.TempMoveDirection = Vector3.zero;
        playerControl.BlendPos = Vector2.zero;
        if (playerControl.RightPressing ^ playerControl.LeftPressing)
        {
            var temp = playerControl.PlayerBody.transform.right;
            playerControl.BlendPos += Vector2.right;
            if (playerControl.LeftPressing)
            {
                playerControl.BlendPos += Vector2.left * 2;
                temp = -temp;
            }
            playerControl.TempMoveDirection += temp;
        }
        if (playerControl.ForwardPressing ^ playerControl.BackPressing)
        {
            var temp = playerControl.PlayerBody.transform.forward;
            playerControl.BlendPos += Vector2.up;
            if (playerControl.BackPressing)
            {
                playerControl.BlendPos += Vector2.down * 2;
                temp = -temp;
            }
            playerControl.TempMoveDirection += temp;
        }

        if(playerControl.TempMoveDirection == Vector3.zero)
        {
            playerStateController.ChangeState(CharacterState.Idle);
        }

        if (playerControl.JumpPressed)
        {
            playerControl.CheckJump = true;
        }

        if (playerControl.DashPressing && playerControl.ForwardPressing)
        {
            playerStateController.ChangeState(CharacterState.Dash);
        }
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
