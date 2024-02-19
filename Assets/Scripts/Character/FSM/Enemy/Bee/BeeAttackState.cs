using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttackState : EnemyBaseFSM
{
    private BeeControl mySelf;

    public BeeAttackState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as BeeControl;
        mySelf.MyNavMeshAgent.speed = 0.0f;
        mySelf.MyNavMeshAgent.updateRotation = false;
        mySelf.MyAttackSign.SetActive(true);
    }

    public override void StateExit()
    {
        mySelf.MyNavMeshAgent.updateRotation = true;
        mySelf.MyAttackSign.SetActive(false);
    }

    public override void StateFixedUpdate()
    {
        if(mySelf.Health > 0)
        {
            var targetnormal = (player.transform.position - mySelf.transform.position).normalized;
            var dot = Vector3.Dot(mySelf.transform.forward, targetnormal);
            if (1.0f - dot > float.Epsilon)
            {
                var rot = Quaternion.LookRotation(player.transform.position - mySelf.transform.position).eulerAngles;
                var result = rot - mySelf.transform.eulerAngles;

                result.x = Mathf.Abs(result.x) > 180.0f ? -(result.x % 180.0f) : result.x;
                result.y = Mathf.Abs(result.y) > 180.0f ? -(result.y % 180.0f) : result.y;
                result.z = Mathf.Abs(result.z) > 180.0f ? -(result.z % 180.0f) : result.z;
                mySelf.transform.eulerAngles += result.normalized * 500.0f * Time.deltaTime;
            }
            else
            {
                mySelf.transform.eulerAngles = Quaternion.LookRotation(player.transform.position - mySelf.transform.position).eulerAngles;
            }
        }
    }

    public override void StateUpdate()
    {
        if (mySelf.Attack)
        {
            if (Vector3.Distance(player.transform.position, mySelf.transform.position) < mySelf.AttackRangeCollider.radius)
            {
                mySelf.Attack = false;
            }
        }

        if (!mySelf.MyAnimator.GetBool("IsAttack"))
        {
            characterStateController.ChangeState(CharacterState.Move);
        }
    }
}
