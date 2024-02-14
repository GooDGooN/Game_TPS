using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimapCamera : MonoBehaviour
{
    private Transform playerBodyTransform;

    private void Start()
    {
        playerBodyTransform = PlayerControl.Instance.PlayerBody.transform;
    }
    private void FixedUpdate()
    {
        if(playerBodyTransform != null)
        {
            transform.position = playerBodyTransform.position + Vector3.up * 10.0f;
            transform.eulerAngles = (Vector3.right * 90.0f) + (Vector3.back * (playerBodyTransform.eulerAngles.y));
        }
    }
}
