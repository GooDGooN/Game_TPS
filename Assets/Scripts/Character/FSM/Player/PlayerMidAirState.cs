using CharacterNamespace;
using Unity.Physics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMidAirState : CharacterBaseFSM
{
    private PlayerControl player;
    public PlayerMidAirState(CharacterStateController stateController, PlayerControl player) : base(stateController, player) { }

    public override void StateEnter()
    {
        player = characterInfo as PlayerControl;

        player.MyAnimator.SetBool("MidAir", true);
        player.MoveDirDampSmooth = 0.2f;
    }

    public override void StateExit()
    {
        player.MyAnimator.SetBool("MidAir", false);
        player.CheckJump = false;
        player.MyRigidbody.velocity = new Vector3(player.MyRigidbody.velocity.x, 0.0f, player.MyRigidbody.velocity.z);
        player.MoveDirDampSmooth = 0.1f;
    }

    public override void StateFixedUpdate()
    {
        Physics.BoxCast(player.MyRigidbody.position, player.BottomCastBox, Vector3.down, out var playerRay, Quaternion.identity, float.PositiveInfinity, Constants.SolidLayer);
        if (playerRay.distance >= player.CapsuleColliderHeight + 0.05f)
        {
            player.MyRigidbody.AddForce(Vector3.down * 3.0f);
        }
        else
        {
            characterStateController.ChangeState(CharacterState.Idle);
            //playerStateController.ChangeState(playerStateController.LastState);
        }
    }

    public override void StateUpdate()
    {
    }
}
