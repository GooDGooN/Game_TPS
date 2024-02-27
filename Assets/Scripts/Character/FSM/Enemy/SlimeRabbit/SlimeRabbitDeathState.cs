using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbitDeathState : EnemyBaseFSM
{
    private SlimeRabbitControl mySelf;
    public SlimeRabbitDeathState(CharacterStateController characterStateController, CharacterProperty characterInfo) : base(characterStateController, characterInfo) { }
    public override void StateEnter()
    {
        mySelf = characterInfo as SlimeRabbitControl;
        mySelf.DeathBurrowDelay();
        if (mySelf.IsSplit)
        {
            mySelf.MyAnimator.SetTrigger("Death");
        }
        else
        {
            for (int i = -1; i < 2; i += 2)
            {
                EnemySpawner.Instance.SpawnSplitSlimeRabbit(mySelf.transform.position);
            }
            mySelf.gameObject.SetActive(false);
        }
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
