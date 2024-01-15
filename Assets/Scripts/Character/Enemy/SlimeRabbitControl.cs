using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SlimeRabbitControl : EnemyProperty
{
    public bool Jumping { get => jumping; set => jumping = value; }
    private bool jumping = false;
    private void Awake()
    {
        PropertySet();
        if(GlobalVarStorage.Instance.PlayerObj != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = playerLayerMask;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        MyNavMeshAgent.speed = moveSpeed;
        MyNavMeshAgent.acceleration = 50.0f;
    }

    private void Update()
    {
        //Debug.Log($"Slime Rabbit health = {health}");
        stateController.CurrentState.StateUpdate();
    }

    private void FixedUpdate()
    {
        stateController.CurrentState.StateFixedUpdate();
    }

    private void PropertySet()
    {
        myType = EnemyType.SlimeRabbit;
        health = 100;
        moveSpeed = 4.0f;
        atkDamage = 0;
        atkSpeed = 0;
    }



    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            myAnimator.SetBool("IsAttack", true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            myAnimator.SetBool("IsAttack", false);
        }
    }

}
