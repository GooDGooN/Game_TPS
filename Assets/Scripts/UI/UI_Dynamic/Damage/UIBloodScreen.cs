using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBloodScreen : MonoBehaviour
{
    private void Update()
    {
        var rectTransform = GetComponent<RectTransform>();
        if(rectTransform.localScale.x < 2.0f)
        {
            rectTransform.localScale += Vector3.one * Time.deltaTime;
        }
    }
}
