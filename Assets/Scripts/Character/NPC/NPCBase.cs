using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    protected PlayerControl player;
    protected UIConversationControl conversationControl;
    protected float interactDistance = 3.0f;

    protected virtual void Start()
    {
        conversationControl = UIConversationControl.Instance;
        player = FindObjectOfType<PlayerControl>();
    }
}
