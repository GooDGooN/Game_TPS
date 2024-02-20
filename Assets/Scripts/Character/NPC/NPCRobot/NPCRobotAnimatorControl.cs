using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRobotAnimatorControl : MonoBehaviour
{
    private Animator myAnimator;
    public float LookWeight = 0.0f;
    public Transform LookTarget;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        LookWeight = Mathf.Clamp(LookWeight, 0.0f, 1.0f);
        if(GameManager.IsGameStart)
        {
            myAnimator.SetTrigger("Leave");
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        myAnimator.SetLookAtWeight(LookWeight);
        myAnimator.SetLookAtPosition(LookTarget.position);
    }

    public void RobotFly()
    {
        transform.parent.GetComponent<NPCRobot>().DoJump = true;
        myAnimator.speed = 0.0f;
    }
}
