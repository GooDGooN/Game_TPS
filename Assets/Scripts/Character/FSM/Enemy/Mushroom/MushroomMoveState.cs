using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMoveState : CharacterBaseFSM
{
    private MushroomControl mySelf;
    private PlayerControl player;
    public MushroomMoveState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as MushroomControl;
        player = GlobalVarStorage.Instance.PlayerScript;
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