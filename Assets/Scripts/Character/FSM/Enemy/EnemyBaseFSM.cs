using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseFSM
{
    protected EnemyControl enemy;
    protected EnemyStateControl enemyStateControl;

    public EnemyBaseFSM (EnemyControl enemy, EnemyStateControl stateControl)
    {
        this.enemy = enemy;
        this.enemyStateControl = enemyStateControl;
    }
}
