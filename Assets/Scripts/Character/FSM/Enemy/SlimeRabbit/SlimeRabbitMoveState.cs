using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitMoveState : EnemyBaseFSM
{
    private SlimeRabbitControl mySelf;
    public SlimeRabbitMoveState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
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
            if (mySelf.MyNavMeshAgent.isActiveAndEnabled)
            {
                mySelf.MyNavMeshAgent.SetDestination(player.transform.position - (Vector3.up * player.CapsuleColliderHeight));
            }
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
