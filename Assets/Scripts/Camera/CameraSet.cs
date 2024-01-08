using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    [SerializeField] private Transform dummy;
    private Vector3 dampPosVelocity = Vector3.zero;
    private Vector3 dampRotVelocity = Vector3.zero;

    private Vector3 targetRot = Vector3.zero;
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, dummy.position) < 0.01f)
        {
            transform.position = dummy.position;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, dummy.position, ref dampPosVelocity, GameSystem.Instance.CameraDampValue);            
            targetRot.x = Mathf.SmoothDampAngle(transform.eulerAngles.x, dummy.eulerAngles.x, ref dampRotVelocity.x, GameSystem.Instance.CameraDampValue);
            targetRot.y = Mathf.SmoothDampAngle(transform.eulerAngles.y, dummy.eulerAngles.y, ref dampRotVelocity.y, GameSystem.Instance.CameraDampValue);
            targetRot.z = Mathf.SmoothDampAngle(transform.eulerAngles.z, dummy.eulerAngles.z, ref dampRotVelocity.z, GameSystem.Instance.CameraDampValue);
            transform.localRotation = Quaternion.Euler(targetRot);
        }

    }
}
