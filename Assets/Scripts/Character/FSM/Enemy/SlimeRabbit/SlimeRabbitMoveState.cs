using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitMoveState : CharacterBaseFSM
{
    private SlimeRabbitControl mySelf;   
    private PlayerControl player;
    public SlimeRabbitMoveState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
        player = GlobalVarStorage.PlayerScript;
        mySelf.MyAnimator.SetBool("IsMove", true);
    }

    public override void StateExit()
    {
        mySelf.MyAnimator.SetBool("IsMove", false);
    }

    public override void StateFixedUpdate()
    {
        if (mySelf.Jumping || mySelf.IsMidAir)
        {
            mySelf.MyNavMeshAgent.SetDestination(player.transform.position - (Vector3.up * player.CapsuleColliderHeight));
        }
    }

    public override void StateUpdate()
    {
        if(mySelf.MyAnimator.GetBool("IsAttack"))
        {
            characterStateController.ChangeState(CharacterState.Attack);
        }
    }
}
