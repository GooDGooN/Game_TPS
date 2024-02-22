using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;
using UnityEngine.AI;

public class BeeDeathState : EnemyBaseFSM
{
    private BeeControl mySelf;
    public BeeDeathState(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo) { }

    public override void StateEnter()
    {
        mySelf = characterInfo as BeeControl;
        mySelf.MyCapsuleCollider.enabled = false;
        mySelf.MyNavMeshAgent.updateRotation = false;
        mySelf.DestroyBall();
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
        mySelf.FlyHeight = mySelf.FlyHeight > 0.0f ? mySelf.FlyHeight - Time.deltaTime * 3.0f : 0.0f;
    }
}
