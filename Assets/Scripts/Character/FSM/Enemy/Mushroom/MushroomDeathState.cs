using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDeathState : EnemyBaseFSM
{
    private MushroomControl mySelf;
    public MushroomDeathState(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo) { }

    public override void StateEnter()
    {
        mySelf = characterInfo as MushroomControl;
        mySelf.MyAnimator.SetTrigger("Death");
        mySelf.DeathBurrowDelay();
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
