using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBat : NPCBase
{
    private bool isLeave = false;

    protected override void Start()
    {
        base.Start();
        NPCName = "Batty";
        MyDialog.Add("Greeting!");
        MyDialog.Add("Welcome to the TPS Survive");
        MyDialog.Add("You can move with pressing keyboard WASD keys");
        MyDialog.Add("While you moving, you can Dash with press keyboard Shift key");
        MyDialog.Add("Press the left mouse button for fire bullet, And you Can reload the magazine with Keyboard R key");
        MyDialog.Add("You can zoom in pressing Right mouse button");
        MyDialog.Add("For Free View, Press mouse middle button");
        MyDialog.Add("That's all, Happy Hunting!");
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < interactDistance)
        {
            transform.eulerAngles = LookPlayerSlowly(transform);
            if (!isLeave)
            {
                if(!dialogControl.NearNPCs.Contains(this))
                {
                    dialogControl.NearNPCs.Add(this);
                }
            }
        }
        else
        {
            dialogControl.NearNPCs.Remove(this);
        }
    }

    public override void EndDialog()
    {
        if(LeaveCoroutine == null)
        {
            LeaveCoroutine = StartCoroutine(Leaving());
            dialogControl.StopConversation = true;
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
        GameManager.IsGameStart = true;
        Destroy(gameObject);
    }
}
