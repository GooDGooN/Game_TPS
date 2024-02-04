using CharacterNamespace;
using System;
using Unity.Physics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class PlayerMoveState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerMoveState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }

    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;

        player.MoveSpeed = player.DefaultMoveSpeed;
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

        if (!player.IsStaminaRecharge)
        {
            if (player.JumpPressed)
            {
                player.CheckJump = true;
            }

            if (player.DashPressing)
            {
                characterStateController.ChangeState(CharacterState.Dash);
            }
        }
    }
    public override void StateFixedUpdate()
    {
        //Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, GlobalVarStorage.SolidLayer);
        Physics.BoxCast(player.MyRigidbody.position, player.BottomCastBox, Vector3.down, out var playerRay,Quaternion.identity, float.PositiveInfinity, GlobalVarStorage.SolidLayer);
        if (playerRay.distance > player.CapsuleColliderHeight + player.ColliderDelta)
        {
            characterStateController.ChangeState(CharacterState.MidAir);
        }
        if (player.CheckJump)
        {
            player.MyRigidbody.AddForce(Vector3.up * player.JumpPower);
            characterStateController.ChangeState(CharacterState.MidAir);
        }
    }
}
