using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandGrip : MonoBehaviour
{
    private Animator myAnimator;
    [SerializeField] private Transform rightHandGrip;
    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private Transform leftHandDashGrip;
    private Transform targetLeftHandTranfrom;
    private float ikRightHandWeight = 1.0f;
    private float ikLeftHandWeight = 1.0f;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (myAnimator.GetBool("Dash"))
        {
            ikRightHandWeight = 0.0f;
            ikLeftHandWeight = 1.0f;
            targetLeftHandTranfrom = leftHandDashGrip;
        }
        else
        {
            targetLeftHandTranfrom = leftHandGrip;
            if (myAnimator.GetBool("Reload"))
            {
                ikRightHandWeight = 1.0f;
                ikLeftHandWeight = 0.0f;
            }
            else
            {
                ikRightHandWeight = 1.0f;
                ikLeftHandWeight = 1.0f;
            }
        }

    }
    private void OnAnimatorIK(int layerIndex)
    {
        myAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandGrip.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikRightHandWeight);
        myAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandGrip.rotation);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikRightHandWeight);

        myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHandTranfrom.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);
        /*myAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrip.rotation);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);*/
    }

    public void PlayerReloadStart()
    {
    }
    public void PlayerReloadEnd()
    {
        myAnimator.SetBool("Reload", false);
    }
}
