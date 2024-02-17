using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BeeControl : EnemyProperty
{
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;
    public GameObject AttackBall { get => attackBall; }
    private GameObject attackBall;
    [SerializeField] private GameObject attackBallPrefab;

    [SerializeField] private GameObject firePoint;

    protected override void OnEnable()
    {
        base.OnEnable();
        flyHeight = 1.5f;
        myType = EnemyType.Bee;
    }

    private void Update()
    {        
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            flyHeight = flyHeight > 0.0f ? flyHeight - Time.deltaTime * 3.0f : 0.0f;
            myCapsuleCollider.enabled = false;
            myNavMeshAgent.updateRotation = false;
            if (!MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if(attackBall != null)
                {
                    if(attackBall.transform.parent != null)
                    {
                        Destroy(attackBall);
                    }
                }
                MyAnimator.SetTrigger("Death");
                StartCoroutine(DeathBurrowDelay());
            }
        }
        transform.localPosition += Vector3.up * flyHeight;
    }

    public void CreateAttackBall()
    {
        attackBall = Instantiate(attackBallPrefab, firePoint.transform.position, Quaternion.identity, transform);
        attackBall.GetComponent<BeeAttackBall>().Initialize(atkDamage, firePoint.transform);
    }

    public void FireAttackBall()
    {
        if(attackBall != null)
        {
            attackBall.transform.parent = null;
            attackBall.transform.LookAt(PlayerControl.Instance.transform.position);
            attackBall.GetComponent<BeeAttackBall>().Fire();
        }
    }
}
