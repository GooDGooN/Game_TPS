using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRobot : NPCBase
{
    private bool isLeave = false;
    [SerializeField] private Transform robotHead;
    private NPCRobotAnimatorControl animatorControl;

    public bool DoJump = false;

    protected override void Start()
    {
        base.Start();
        NPCName = "Botty";

        animatorControl = GetComponentInChildren<NPCRobotAnimatorControl>();

        Physics.Raycast(transform.position, Vector3.down, out var groundHit, float.PositiveInfinity, Constants.SolidLayer);
        transform.position = groundHit.point;

        MyDialog.Add($"Your highest kill count is {PlayerPrefs.GetInt("HighestKillCount")} kills");
        MyDialog.Add($"...And your longest survive time is {PlayerPrefs.GetFloat("LongestSurviveTime")} sec");
        MyDialog.Add($"...That's it");
        MyDialog.Add($"...");
        MyDialog.Add($"...");
        MyDialog.Add($"...What?");
    }

    protected override void Update()
    {
        base.Update();
        bool isInRange = Vector3.Distance(transform.position, player.transform.position) < interactDistance;

        bool isInSight = Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized) > 0.3f;

        animatorControl.LookWeight += isInRange && isInSight ? Time.deltaTime : -Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) < interactDistance)
        {
            if(!isLeave)
            {
                if (!dialogControl.NearNPCs.Contains(this))
                {
                    dialogControl.NearNPCs.Add(this);
                }
            }
        }
        else
        {
            dialogControl.NearNPCs.Remove(this);
        }

        if(DoJump)
        {
            if (LeaveCoroutine == null)
            {
                LeaveCoroutine = StartCoroutine(Leaving());
            }
        }
    }
    private IEnumerator Leaving()
    {
        float time = 0.0f;
        while(time < 1.0f)
        {
            transform.position += Vector3.up * Time.deltaTime * 20.0f;
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }    


}

