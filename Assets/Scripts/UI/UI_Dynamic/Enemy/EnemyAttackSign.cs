using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackSign : MonoBehaviour
{
    public EnemyProperty MyTarget;
    private RectTransform myRectTransform;
    private Color myColor;

    private void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
        myColor = GetComponent<Image>().color;
    }

    private void Update()
    {
        if (MyTarget.isActiveAndEnabled)
        {
            transform.position = Camera.main.WorldToScreenPoint(MyTarget.transform.position + Vector3.up * 1.5f);
            myColor.a = myRectTransform.position.z < 0.0f ? 0.0f : 1.0f;
            GetComponent<Image>().color = myColor;

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
