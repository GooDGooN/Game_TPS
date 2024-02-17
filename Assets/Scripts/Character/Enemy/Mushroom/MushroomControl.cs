using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomControl : EnemyProperty
{
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        myType = EnemyType.Mushroom;
    }


    private void Update()
    {
        //Debug.Log($"Slime Rabbit health = {health}");
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            if (!MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                MyAnimator.SetTrigger("Death");
                StartCoroutine(DeathBurrowDelay());
            }
        }
    }
}
