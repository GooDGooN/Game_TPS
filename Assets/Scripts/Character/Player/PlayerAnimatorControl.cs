using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimatorControl : MonoBehaviour
{
    #region GENERAL
    private Animator myAnimator;
    private PlayerControl playerControl;
    private Vector3 targetBodyPosition = Vector3.down * 0.75f;
    #endregion

    #region IK_HAND
    [SerializeField] private Transform rightHandGrip;
    [SerializeField] private Transform leftHandGrip;
    [SerializeField] private Transform leftHandDashGrip;

    private Transform targetLeftHandTranfrom;
    private float ikRightHandWeight = 1.0f;
    private float ikLeftHandWeight = 1.0f;
    #endregion

    #region IK_FOOT
    [SerializeField] private Transform rightFootBottom;
    [SerializeField] private Transform leftFootBottom;
    private float ikFootCheckDist = 0.5f;
    private Vector3 ikFootDampVelocity = Vector3.zero;
    private float ikFootDampSmooth = 0.1f;
    #endregion


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControl = GetComponentInParent<PlayerControl>();
    }

    private void Update()
    {
        #region IK_HAND
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
        #endregion

        #region POSITION_SMOOTHDAMP
        ikFootDampSmooth = (playerControl.MyState == CharacterState.Idle) ? 0.1f : 0.25f;
        transform.position = Vector3.SmoothDamp(transform.position, targetBodyPosition, ref ikFootDampVelocity, ikFootDampSmooth);
        if (Vector3.Distance(transform.position, targetBodyPosition) < 0.01f)
        {
            transform.position = targetBodyPosition;
        }
        #endregion
    }
    private void OnAnimatorIK(int layerIndex)
    {
        #region FOOTIK
        var tempPosSave = transform.position;
        transform.localPosition = Vector3.up * -0.75f;

        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.0f);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.0f);

        if (playerControl.MyState == CharacterState.Idle)
        {
            Physics.Raycast(leftFootBottom.position, Vector3.down, out var leftFootHit, ikFootCheckDist, Constants.SolidLayer);
            Physics.Raycast(rightFootBottom.position, Vector3.down, out var rightFootHit, ikFootCheckDist, Constants.SolidLayer);
            var deltaDist = leftFootHit.distance > rightFootHit.distance ? leftFootHit.distance : rightFootHit.distance;

            transform.position += Vector3.down * deltaDist;

            if (leftFootHit.point != Vector3.zero && leftFootHit.distance > 0.1f)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
            }
            if (rightFootHit.point != Vector3.zero && rightFootHit.distance > 0.1f)
            {
                myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            }

            var leftFootIKPos = leftFootBottom.position + Vector3.up * (rightFootHit.distance);
            var rightFootIKPos = rightFootBottom.position + Vector3.up * (leftFootHit.distance);

            myAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPos);
            myAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKPos);
        }
        targetBodyPosition = transform.position;
        transform.position = tempPosSave;
        #endregion

        #region HANDIK
        myAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandGrip.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikRightHandWeight);
        myAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandGrip.rotation);
        myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikRightHandWeight);

        myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, targetLeftHandTranfrom.position);
        myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);
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
        playerControl.ReloadComplete = true;
    }


}
