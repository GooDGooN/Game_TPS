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
    private void OnEnable()
    {
        myPosition = transform.position;
        GetComponent<TMP_Text>().alpha = 1.0f;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InOutElastic);
        StartCoroutine(DeactivateDelay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        GetComponent<TMP_Text>().alpha = 1.0f;
        GetComponent<TMP_Text>().text = "";
        transform.DOKill();
    }

    private IEnumerator DeactivateDelay()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<TMP_Text>().DOFade(0.0f, 0.5f);
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(myPosition);

        if (GetComponent<TMP_Text>().alpha <= 0.0f)
        {
            gameObject.SetActive(false);
        }
    }
}
