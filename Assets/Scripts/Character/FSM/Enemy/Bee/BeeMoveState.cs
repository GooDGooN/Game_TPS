using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMoveState : CharacterBaseFSM
{
    private BeeControl mySelf;
    private PlayerControl player;
    public BeeMoveState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }

    public override void StateEnter()
    {
        mySelf = characterInfo as BeeControl;
        player = GlobalVarStorage.PlayerScript;
        mySelf.MyAnimator.SetBool("IsMove", true);
        mySelf.MyNavMeshAgent.speed = mySelf.MoveSpeed;
    }

    public override void StateExit()
    {
        mySelf.MyAnimator.SetBool("IsMove", false);
    }

    public override void StateFixedUpdate()
    {
        if (mySelf.Health > 0 || mySelf.IsMidAir)
        {
            mySelf.MyNavMeshAgent.SetDestination(player.transform.position - (Vector3.up * player.CapsuleColliderHeight));
        }
    }

    public override void StateUpdate()
    {
        if (mySelf.MyAnimator.GetBool("IsAttack"))
        {
            characterStateController.ChangeState(CharacterState.Attack);
        }
    }
}
