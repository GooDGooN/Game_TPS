using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class UIDamageText : MonoBehaviour
{
    private Vector3 myPosition = Vector3.zero;
    private Vector3 smoothDampVelocity = Vector3.zero;
    private TMP_Text myTMPtext;
    public Vector3 targetPosition;

    private void Awake()
    {
        myTMPtext = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        myPosition = transform.position;
        myTMPtext.alpha = 1.0f;
        transform.localScale = Vector3.zero;
        var randomScaleValue = Random.Range(1.5f, 2.0f);
        transform.DOScale(Vector3.one * randomScaleValue, 0.5f).SetEase(Ease.OutElastic);
        StartCoroutine(DeactivateDelay());

    }

    private void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        myTMPtext.alpha = 1.0f;
        myTMPtext.text = "";
        transform.DOKill();
    }

    private IEnumerator DeactivateDelay()
    {
        yield return new WaitForSeconds(1.0f);
        myTMPtext.DOFade(0.0f, 0.5f);
    }

    private void Update()
    {
        myPosition = Vector3.SmoothDamp(myPosition, targetPosition, ref smoothDampVelocity, 0.05f);
        transform.position = Camera.main.WorldToScreenPoint(myPosition);
        
        // Clamp the trasform.position
        var clampPosition = GetComponent<RectTransform>().anchoredPosition;
        var canvasWitdh = transform.parent.GetComponent<RectTransform>().rect.width;
        var canvasheight = transform.parent.GetComponent<RectTransform>().rect.height;
        var screenHalfWH = new Vector2(canvasWitdh * 0.5f, canvasheight * 0.5f);
        screenHalfWH -= Vector2.one * myTMPtext.fontSize;

        clampPosition.x = Mathf.Clamp(clampPosition.x, -screenHalfWH.x, screenHalfWH.x);
        clampPosition.y = Mathf.Clamp(clampPosition.y, -screenHalfWH.y, screenHalfWH.y);
        GetComponent<RectTransform>().anchoredPosition = clampPosition;




        if (myTMPtext.alpha < 0.01f)
        {
            gameObject.SetActive(false);
        }
    }
}

