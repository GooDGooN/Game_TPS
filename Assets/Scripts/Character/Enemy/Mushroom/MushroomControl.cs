using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomControl : EnemyProperty
{
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;

    private void OnEnable()
    {
        myType = EnemyType.Mushroom;
        PropertySet();
        if (GlobalVarStorage.Instance.PlayerObj != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = GlobalVarStorage.Instance.PlayerLayer;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        MyNavMeshAgent.speed = moveSpeed;
        MyNavMeshAgent.acceleration = 50.0f;
    }


    private void Update()
    {
        //Debug.Log($"Slime Rabbit health = {health}");
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            MyAnimator.SetTrigger("Death");
            StartCoroutine(DeathBurrowDelay());
        }
    }
}
