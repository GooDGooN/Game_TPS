using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BeeControl : EnemyProperty
{
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;
    public GameObject AttackBall;
    [SerializeField] private GameObject attackBallPrefab;

    [SerializeField] private GameObject firePoint;

    protected override void OnEnable()
    {
        base.OnEnable();
        FlyHeight = 1.5f;
        myType = EnemyType.Bee;
    }

    private void Update()
    {        
        stateController.CurrentState.StateUpdate();
        transform.localPosition += Vector3.up * FlyHeight;
    }

    public void CreateAttackBall()
    {
        AttackBall = Instantiate(attackBallPrefab, firePoint.transform.position, Quaternion.identity, transform);
        AttackBall.GetComponent<BeeAttackBall>().Initialize(atkDamage, firePoint.transform);
    }

    public void FireAttackBall()
    {
        if(AttackBall != null)
        {
            AttackBall.transform.parent = null;
            AttackBall.transform.LookAt(PlayerControl.Instance.transform.position);
            AttackBall.GetComponent<BeeAttackBall>().Fire();
        }
    }

    public void DestroyBall()
    {
        if (AttackBall != null)
        {
            if (AttackBall.transform.parent != null)
            {
                Destroy(AttackBall);
            }
        }
    }
}
