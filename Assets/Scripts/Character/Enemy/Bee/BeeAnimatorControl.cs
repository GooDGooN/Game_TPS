using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimatorControl : MonoBehaviour
{
    private BeeControl parent;
    private void Awake()
    {
        parent = GetComponentInParent<BeeControl>();
    }
    public void BeeAttack()
    {
        if (parent.MyState == CharacterState.Attack)
        {
            parent.Attack = true;
        }
    }

    public void BeeCreateBall()
    {
        parent.CreateAttackBall();
    }

    public void BeeFireBall()
    {
        parent.FireAttackBall();
    }
}
