using CharacterNamespace;
using System;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class PlayerMoveState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerMoveState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }

    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;

        player.MoveSpeed = 200.0f;
    }

    public override void StateExit()
    {
        /*playerctr.TempMoveDirection = Vector3.zero;
        playerctr.BlendPos = Vector2.zero;*/
    }

    public override void StateUpdate()
    {
        if (player.TempMoveDirection == Vector3.zero)
        {
            characterStateController.ChangeState(CharacterState.Idle);
        }

        if (player.JumpPressed)
        {
            player.CheckJump = true;
        }

        if (player.DashPressing && player.ForwardPressing)
        {
            characterStateController.ChangeState(CharacterState.Dash);
        }
    }
    public override void StateFixedUpdate()
    {
        Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, GlobalVarStorage.Instance.SolidLayer);
        if (playerRay.distance > player.CapsuleColliderHeight + 0.2f)
        {
            characterStateController.ChangeState(CharacterState.MidAir);
        }
        if (player.CheckJump)
        {
            player.MyRigidbody.AddForce(Vector3.up * player.JumpPower);
            characterStateController.ChangeState(CharacterState.MidAir);
        }

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
    }
}
