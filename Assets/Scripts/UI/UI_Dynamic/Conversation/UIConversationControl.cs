using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIConversationControl : Singleton<UIConversationControl>
{
    public TMP_Text ChatText;
    public GameObject InteractText;
    public GameObject ChatBackground;
    public bool InConversation = false;
    public GameObject CurrentConversationTarget;

    public GameObject[] ToggleObjs; 

    private string currentText;
    private int stringIndex = 0;

    public List<GameObject> NearNPCs = new List<GameObject>();

    private Coroutine currentCoroutine = null;

    private void Update()
    {
        if (InConversation)
        {
            ChatBackground.SetActive(true);
            if (ToggleObjs[ToggleObjs.Length - 1].activeSelf) 
            {
                foreach (GameObject obj in ToggleObjs)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            if (!ToggleObjs[ToggleObjs.Length - 1].activeSelf)
            {
                foreach (GameObject obj in ToggleObjs)
                {
                    obj.SetActive(true);
                }
            }
            ChatBackground.SetActive(false);
            if (NearNPCs.Count > 0)
            {
                InteractText.SetActive(true);
            }
            else
            {
                InteractText.SetActive(false);
            }
        }
    }

    private IEnumerator DrawingText()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (stringIndex < currentText.Length)
            {
                ChatText.text += currentText[stringIndex++];
            }
            else
            {
                break;
            }
        }
    }

    public void SetText(string text, bool isContinue = false)
    {
        Cleartext();
        currentCoroutine = StartCoroutine(DrawingText());
        stringIndex = 0;
        ChatText.text = isContinue ? currentText : "";
        currentText = text;
    }

    private void Cleartext()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = null;
    }
}
