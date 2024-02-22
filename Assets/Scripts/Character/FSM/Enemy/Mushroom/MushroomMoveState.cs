using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMoveState : EnemyBaseFSM
{
    private MushroomControl mySelf;
    public MushroomMoveState(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as MushroomControl;
        mySelf.MyAnimator.SetBool("IsMove", true);
    }
    public override void StateExit()
    {
        mySelf.MyAnimator.SetBool("IsMove", false);
    }

    public override void StateFixedUpdate()
    {
        if(mySelf.Health > 0 || mySelf.IsMidAir)
        {
            if (mySelf.MyNavMeshAgent.isActiveAndEnabled)
            {
                mySelf.MyNavMeshAgent.SetDestination(player.transform.position - (Vector3.up * player.CapsuleColliderHeight));
            }
        }    
    }

    public override void StateUpdate()
    {
        if (mySelf.MyAnimator.GetBool("IsAttack"))
        {
            characterStateController.ChangeState(CharacterState.Attack);
        }

        if (mySelf.Health <= 0)
        {
            characterStateController.ChangeState(CharacterState.Death);
        }
    }
}
