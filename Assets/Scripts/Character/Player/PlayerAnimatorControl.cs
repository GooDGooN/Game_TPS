using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimatorControl : MonoBehaviour
{
    [SerializeField] private Transform rightHandGrip;
    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private Transform leftHandDashGrip;

    private PlayerControl parent;
    [SerializeField] private Animator myAnimator;
    private Transform targetLeftHandTranfrom;
    private float ikRightHandWeight = 1.0f;
    private float ikLeftHandWeight = 1.0f;

    private float footIKStartPosDelta = 1.0f;
    private float footIKCheckDist = 0.5f;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        parent = GetComponentInParent<PlayerControl>();
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
        #region HANDIK
        myAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandGrip.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikRightHandWeight);
        myAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandGrip.rotation);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikRightHandWeight);

        myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHandTranfrom.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);
        /*myAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrip.rotation);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);*/
        #endregion

        #region FOOTIK
        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);

        myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.0f);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.0f);
        Physics.Raycast(myAnimator.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down, out var leftFootHit, footIKCheckDist, Constants.SolidLayer);
        Physics.Raycast(myAnimator.GetIKPosition(AvatarIKGoal.RightFoot), Vector3.down, out var rightFootHit, footIKCheckDist, Constants.SolidLayer);
        if(parent.MyState == CharacterState.Idle)
        {
            if(leftFootHit.point != Vector3.zero && leftFootHit.distance > 0.1f)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
            }
            if (rightFootHit.point != Vector3.zero && rightFootHit.distance > 0.1f)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            }
        }
        var leftFootIKPos = myAnimator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up * (rightFootHit.distance - 0.1f);
        var rightFootIKPos = myAnimator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up * (leftFootHit.distance - 0.1f);

        myAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPos);
        myAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKPos);
        #endregion
    }

    public void PlayerReloadStart()
    {
    }
    public void PlayerReloadEnd()
    {
        myAnimator.SetBool("Reload", false);
    }

    public void PlayerMagazineChanged()
    {
        parent.ReloadComplete = true;
    }

}
