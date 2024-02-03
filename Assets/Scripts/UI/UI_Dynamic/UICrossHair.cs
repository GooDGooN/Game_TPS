using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrossHair : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void LateUpdate()
    {
        var canvas = transform.parent.GetComponent<Canvas>();

        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}
