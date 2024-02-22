using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitAttackState : EnemyBaseFSM
{
    private SlimeRabbitControl mySelf;
    public SlimeRabbitAttackState(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
        mySelf.MyAttackSign.SetActive(true);
    }

    public override void StateExit()
    {
        mySelf.MyAttackSign.SetActive(false);
    }

    public override void StateFixedUpdate()
    {
        
    }

    public override void StateUpdate()
    {
        if (mySelf.Attack)
        {
            if (Vector3.Distance(player.transform.position, mySelf.transform.position) < mySelf.AttackRangeCollider.radius * 2.0f)
            {
                player.GetDamage(mySelf.AtkDamage);
                mySelf.Attack = false;
            }
        }

        if (!mySelf.MyAnimator.GetBool("IsAttack") && mySelf.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.8f)
        {
            characterStateController.ChangeState(CharacterState.Move);
        }

        if (mySelf.Health <= 0)
        {
            characterStateController.ChangeState(CharacterState.Death);
        }
    }
}
