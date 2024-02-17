using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyMinimapIcon : MonoBehaviour
{
    private GameObject myTarget;
    private RectTransform myRectTranform;
    public Camera minimapCamera;
    public void Initialize(GameObject targetObj)
    {
        myTarget = targetObj;
    }

    private void Awake()
    {
        myRectTranform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (myTarget.GetComponent<EnemyProperty>().Health > 0)
        {
            myRectTranform.anchoredPosition = minimapCamera.WorldToViewportPoint(myTarget.transform.position) * 300.0f;
            //myRectTranform.anchoredPosition -= Vector2.one * 75.0f;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
