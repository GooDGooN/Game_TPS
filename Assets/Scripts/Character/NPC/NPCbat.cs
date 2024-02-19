using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBat : NPCBase
{
    private List<string> myConversation = new List<string>();
    private bool isLeave = false;
    private int conversationNum = 0;

    protected override void Start()
    {
        base.Start();
        myConversation.Add("Greeting!");
        myConversation.Add("Welcome to the TPS Survive");
        myConversation.Add("You can move with pressing keyboard WASD keys");
        myConversation.Add("While you moving, you can Dash with press keyboard Shift key");
        myConversation.Add("Press the left mouse button for fire bullet, And you Can reload the magazine with Keyboard R key");
        myConversation.Add("You can zoom in pressing Right mouse button");
        myConversation.Add("For Free View, Press mouse middle button");
        myConversation.Add("That's all, Happy Hunting!");
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < interactDistance)
        {
            var targetnormal = (player.transform.position - transform.position).normalized;
            var dot = Vector3.Dot(transform.forward, targetnormal);
            if (1.0f - dot > 0.05f)
            {
                var rot = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
                var result = rot - transform.eulerAngles;

                result.x = Mathf.Abs(result.x) > 180.0f ? -(result.x % 180.0f) : result.x;
                result.y = Mathf.Abs(result.y) > 180.0f ? -(result.y % 180.0f) : result.y;
                result.z = Mathf.Abs(result.z) > 180.0f ? -(result.z % 180.0f) : result.z;
                transform.eulerAngles += result.normalized * 250.0f * Time.deltaTime;
            }
            else
            {
                transform.eulerAngles = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
            }
            if(!conversationControl.InConversation && !isLeave)
            {
                if(!conversationControl.NearNPCs.Contains(gameObject))
                {
                    conversationControl.NearNPCs.Add(gameObject);
                }
                if (GameSystem.GetKeyPressed(KeyInputs.Interact))
                {
                    conversationControl.InConversation = true;
                    conversationControl.CurrentConversationTarget = gameObject;
                    conversationControl.SetText(myConversation[conversationNum++]);
                }
            }
            else
            {
                if (GameSystem.GetKeyPressed(KeyInputs.Interact) && !isLeave)
                {
                    conversationNum++;
                    if (myConversation.Count <= conversationNum)
                    {
                        conversationNum = 0;
                        conversationControl.NearNPCs.Remove(gameObject);
                        conversationControl.InConversation = false;
                        conversationControl.CurrentConversationTarget = null;
                        isLeave = true;
                        StartCoroutine(Leaving());
                    }
                    conversationControl.SetText(myConversation[conversationNum]);
                }
            }
        }
        else
        {
            conversationControl.NearNPCs.Remove(gameObject);
            conversationControl.InConversation = false;
            conversationNum = 0;
        }
    }

    private IEnumerator Leaving()
    {
        var dist = 15.0f;
        var targetPos = transform.position + Vector3.up * dist;
        transform.DOMove(targetPos, 1.5f).SetEase(Ease.InOutBack);
        while (true) 
        {
            yield return null;
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                break;
            }
        }
        transform.DOKill();
        GameManager.Instance.IsGameStart = true;
        Destroy(gameObject);
    }
}
