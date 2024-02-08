using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIConversationControl : Singleton<UIConversationControl>
{
    public TMP_Text ChatText;
    public GameObject InteractText;
    public GameObject ChatBackground;
    public bool InConversation = false;

    public List<GameObject> NearNPCs = new List<GameObject>();

    private void Update()
    {
        if (InConversation)
        {
            ChatBackground.SetActive(true);
        }
        else
        {
            ChatBackground.SetActive(false);
        }
        if (NearNPCs.Count > 0)
        {
            InteractText.SetActive(true);
        }
        else
        {
            InteractText.SetActive(false);
        }
    }

    public void SetText(string text) => ChatText.text = text;
}
