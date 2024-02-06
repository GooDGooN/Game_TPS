using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitAttackState : EnemyBaseFSM
{
    private SlimeRabbitControl mySelf;
    public SlimeRabbitAttackState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void StateUpdate()
    {
        if(mySelf.Attack)
        {
            if(Vector3.Distance(player.transform.position, mySelf.transform.position) < mySelf.AttackRangeCollider.radius)
            {
                player.GetDamage(mySelf.AtkDamage);
                mySelf.Attack = false;
            }
        }

        if (!mySelf.MyAnimator.GetBool("IsAttack"))
        {
            characterStateController.ChangeState(CharacterState.Move);
        }
    }
}
