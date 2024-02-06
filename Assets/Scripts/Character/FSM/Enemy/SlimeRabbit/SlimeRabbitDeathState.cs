using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitDeathState : EnemyBaseFSM
{
    private SlimeRabbitControl mySelf;
    public SlimeRabbitDeathState(CharacterStateController stateController, CharacterProperty enemy) : base(stateController, enemy) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
        mySelf.MoveSpeed = 0.0f;
    }

    public override void StateExit()
    {
    }

    public override void StateFixedUpdate()
    {
    }

    public override void StateUpdate()
    {
        
    }
}
