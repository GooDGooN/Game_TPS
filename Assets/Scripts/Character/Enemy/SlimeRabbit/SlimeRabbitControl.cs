using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SlimeRabbitControl : EnemyProperty
{
    public bool Jumping { get => jumping; set => jumping = value; }
    private bool jumping = false;
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;
    public bool IsSplit { get => isSplit; set => isSplit = value; }
    private bool isSplit = false;

    private void OnEnable()
    {
        myType = EnemyType.SlimeRabbit;
        myCapsuleCollider.isTrigger = false;
        PropertySet();
        if (isSplit)
        {
            health /= 2;
            atkSpeed *= 2;
        }
        if(PlayerControl.Instance != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = Constants.PlayerLayer;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        MyNavMeshAgent.speed = moveSpeed;
        MyNavMeshAgent.acceleration = 50.0f;

        if(isSplit)
        {
            transform.localScale = Vector3.one * 0.75f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnDisable()
    {
        isSplit = false;
    }


    private void Update()
    {
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            myCapsuleCollider.isTrigger = true;
            if (IsSplit)
            {
                MyAnimator.SetTrigger("Death");
                StartCoroutine(DeathBurrowDelay());
            }
            else
            {
                for(int i = -1; i < 2; i += 2)
                {
                    EnemyObjectPool.Instance.SlimeRabbitSpawnTest(transform.position, true);
                }
                gameObject.SetActive(false);
            }
        }
        if(isMidAir || (jumping && health > 0))
        {
            myNavMeshAgent.speed = moveSpeed;
        }
        else
        {
            myNavMeshAgent.speed = 0.0f;
        }
    }
}
