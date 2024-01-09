using CharacterNamespace;
using UnityEngine;
using static GlobalEnums;

public class PlayerMidAirState : PlayerBaseFSM
{
    public PlayerMidAirState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }

    public override void StateEnter()
    {
        player.MyAnimator.SetBool("MidAir", true);
    }

    public override void StateExit()
    {
        player.MyAnimator.SetBool("MidAir", false);
        player.CheckJump = false;
        player.MyRigidbody.velocity = new Vector3(player.MyRigidbody.velocity.x, 0.0f, player.MyRigidbody.velocity.z);
    }

    public override void StateFixedUpdate()
    {
        Physics.Raycast(player.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, player.SolidLayer);
        if (playerRay.distance >= player.ColliderHeight + 0.1f)
        {
            player.MyRigidbody.AddForce(Vector3.down * 3.0f);
        }
        else
        {
            playerStateController.ChangeState(CharacterState.Idle);
            //playerStateController.ChangeState(playerStateController.LastState);
        }
    }

    public override void StateUpdate()
    {
    }
}
