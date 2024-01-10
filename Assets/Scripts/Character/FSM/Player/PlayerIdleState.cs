
using CharacterNamespace;
using Unity.Physics;
using UnityEngine;
using static GlobalEnums;

public class PlayerIdleState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerIdleState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }
    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;

        player.TempMoveDirection = Vector3.zero;
        player.BlendPos = Vector2.zero;
    }
    public override void StateExit()
    {
    }
    public override void StateUpdate()
    {
        if (player.RightPressing ^ player.LeftPressing || player.ForwardPressing ^ player.BackPressing)
        {
            characterStateController.ChangeState(CharacterState.Move);
        }
        if (player.JumpPressed)
        {
            player.CheckJump = true;
        }
        player.MyRigidbody.velocity = new Vector3(player.MyRigidbody.velocity.x, 0.0f, player.MyRigidbody.velocity.z);
    }
    public override void StateFixedUpdate() 
    {
        Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, player.SolidLayer);
        if (playerRay.distance > player.ColliderHeight + 0.2f)
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
