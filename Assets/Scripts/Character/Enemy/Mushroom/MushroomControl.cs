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
        stateController.CurrentState.StateUpdate();
    }
}
