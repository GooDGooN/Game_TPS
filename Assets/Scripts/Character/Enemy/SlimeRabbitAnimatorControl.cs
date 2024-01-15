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
        parent.MyNavMeshAgent.speed = parent.MoveSpeed;
        parent.Jumping = true;
    }
    public void SlimeRabbitEndJump()
    {
        parent.MyNavMeshAgent.speed = 0.0f;
        parent.Jumping = false;
    }
}
