using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeRabbitAnimatorControl : MonoBehaviour
{
    private SlimeRabbitControl parent;
    private void Awake()
    {
        parent = GetComponentInParent<SlimeRabbitControl>();
    }
    public void SlimeRabbitStartJump()
    {
        parent.Jumping = true;
    }

    public void SlimeRabbitEndJump()
    {
        parent.Jumping = false;
    }

    public void SlimeRabbitAttack()
    {
        if(parent.MyState == CharacterState.Attack)
        {
            parent.Attack = true;
        }
    }
}
