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
        player.MoveSpeed = 200.0f;
    }

    public override void StateExit()
    {
        /*playerctr.TempMoveDirection = Vector3.zero;
        playerctr.BlendPos = Vector2.zero;*/
    }

    public override void StateUpdate()
    {
        player.TempMoveDirection = Vector3.zero;
        player.BlendPos = Vector2.zero;
        if (player.RightPressing ^ player.LeftPressing)
        {
            var temp = player.PlayerBody.transform.right;
            player.BlendPos += Vector2.right;
            if (player.LeftPressing)
            {
                player.BlendPos += Vector2.left * 2;
                temp = -temp;
            }
            player.TempMoveDirection += temp;
        }
        if (player.ForwardPressing ^ player.BackPressing)
        {
            var temp = player.PlayerBody.transform.forward;
            player.BlendPos += Vector2.up;
            if (player.BackPressing)
            {
                player.BlendPos += Vector2.down * 2;
                temp = -temp;
            }
            player.TempMoveDirection += temp;
        }

        if(player.TempMoveDirection == Vector3.zero)
        {
            playerStateController.ChangeState(CharacterState.Idle);
        }

        if (player.JumpPressed)
        {
            player.CheckJump = true;
        }

        if (player.DashPressing && player.ForwardPressing)
        {
            playerStateController.ChangeState(CharacterState.Dash);
        }
    }
    public override void StateFixedUpdate()
    {
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
}
