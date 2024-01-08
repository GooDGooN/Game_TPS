using CharacterNamespace;
using UnityEngine;
using static GlobalEnums;

public class PlayerMidAirState : PlayerBaseFSM
{
    public PlayerMidAirState(PlayerControl target, PlayerStateController stateController) : base(target, stateController) { }

    private float delay;
    public override void StateEnter()
    {
        playerControl.MyAnimator.SetBool("MidAir", true);
    }

    public override void StateExit()
    {
        playerControl.MyAnimator.SetBool("MidAir", false);
        playerControl.CheckJump = false;
        playerControl.MyRigidbody.velocity = new Vector3(playerControl.MyRigidbody.velocity.x, 0.0f, playerControl.MyRigidbody.velocity.z);
    }

    public override void StateFixedUpdate()
    {
        Physics.Raycast(playerControl.MyRigidbody.position, Vector3.down, out var playerRay, float.PositiveInfinity, playerControl.SolidLayer);
        if (playerRay.distance > playerControl.ColliderHeight + 0.1f)
        {
            playerControl.MyRigidbody.AddForce(Vector3.down * 3.0f);
        }
        else
        {
            playerStateController.ChangeState(playerStateController.LastState);
        }
    }

    public override void StateUpdate()
    {
    }
}
