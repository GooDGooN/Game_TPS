using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProperty : CharacterProperty
{
    public bool IsMidAir { get => isMidAir; }
    protected bool isMidAir = false;

    protected IEnumerator DeathBurrowDelay()
    {
        moveSpeed = 0.0f;
        yield return new WaitForSeconds(3.0f);
        var time = 3.0f;
        while (true)
        {
            transform.position += Vector3.down * Time.deltaTime;
            time -= Time.deltaTime;
            yield return null;
            if (time <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public SphereCollider AttackRangeCollider { get => attackRangeCollider; }
    protected SphereCollider attackRangeCollider
    {
        get
        {
            var comp = GetComponent<SphereCollider>();
            if(comp == null)
            {
                comp = GetComponentInChildren<SphereCollider>();
            }
            return comp;
        }
    }

    public NavMeshAgent MyNavMeshAgent { get => myNavMeshAgent; }
    protected NavMeshAgent myNavMeshAgent
    {
        get
        {
            var nav = GetComponent<NavMeshAgent>();
            if(nav == null) 
            { 
                nav = GetComponentInChildren<NavMeshAgent>();
            }
            return nav;
        }
    }

    public EnemyType MyType { get => myType; }
    [SerializeField] protected EnemyType myType;

    protected virtual void FixedUpdate()
    {
        stateController.CurrentState.StateFixedUpdate();
        Physics.BoxCast(transform.position + new Vector3(0.0f, capsuleColliderHeight, 0.0f), new Vector3(0.4f, 0.01f, 0.4f), Vector3.down, out var info, Quaternion.identity, float.PositiveInfinity, Constants.SolidLayer);
        if (info.distance != 0.0f && info.distance >= capsuleColliderHeight + 0.1f)
        {
            isMidAir = true;
            myAnimator.SetBool("IsMidAir", isMidAir);
            myAnimator.Play("MidAir");
        }
        else
        {
            isMidAir = false;
            myAnimator.SetBool("IsMidAir", isMidAir);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & Constants.PlayerLayer) != 0)
        {
            myAnimator.SetBool("IsAttack", true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & Constants.PlayerLayer) != 0)
        {
            myAnimator.SetBool("IsAttack", false);
        }
    }

    protected void PropertySet()
    {
        switch(myType)
        {
            case EnemyType.SlimeRabbit:
                health = 3;
                moveSpeed = 4.0f;
                atkDamage = 0;
                atkSpeed = 0;
                break;
            case EnemyType.Mushroom:
                health = 5;
                moveSpeed = 3.0f;
                atkDamage = 0;
                atkSpeed = 0;
                break;
            case EnemyType.Bee:
                health = 4;
                moveSpeed = 2.0f;
                atkDamage = 0;
                atkSpeed = 0;
                break;

        }
    }
}
