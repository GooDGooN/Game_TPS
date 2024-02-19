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

    public float FlyHeight { get => flyHeight; }
    protected float flyHeight = 0.0f;

    public bool IsBurrow { get => isBurrow; }
    protected bool isBurrow = false;

    #region COMPONENTS

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
    #endregion 

    public EnemyType MyType { get => myType; }
    [SerializeField] protected EnemyType myType;

    public GameObject HealthBarPrefab;
    protected GameObject myHealthBar;

    public GameObject MyAttackSignPrefab;
    public GameObject MyAttackSign;

    protected virtual void OnEnable()
    {
        if(myHealthBar == null)
        {
            myHealthBar = Instantiate(HealthBarPrefab, EnemyStatusUI.Instance.HealthBarStorage.transform);
            myHealthBar.GetComponent<EnemyHealthBar>().MyTarget = this;
        }

        if(MyAttackSign == null)
        {
            MyAttackSign = Instantiate(MyAttackSignPrefab, EnemyStatusUI.Instance.AttackSignStorage.transform);
            MyAttackSign.GetComponent<EnemyAttackSign>().MyTarget = this;
            MyAttackSign.SetActive(false);
        }

        PropertySet();
        OnEnableSet();
    }

    protected virtual void FixedUpdate()
    {
        stateController.CurrentState.StateFixedUpdate();
        if(!IsBurrow)
        {
            Physics.BoxCast(transform.position + new Vector3(0.0f, capsuleColliderHeight, 0.0f), new Vector3(0.4f, 0.01f, 0.4f), Vector3.down, out var groundHit, Quaternion.identity, float.PositiveInfinity, Constants.SolidLayer);
            if (groundHit.distance != 0.0f && groundHit.distance >= capsuleColliderHeight + 0.1f + flyHeight)
            {
                isMidAir = true;
                if (health > 0)
                {
                    myAnimator.SetBool("IsMidAir", isMidAir);
                    myAnimator.Play("MidAir");
                }
            }
            else
            {
                isMidAir = false;
                myAnimator.SetBool("IsMidAir", isMidAir);
            }
        }

        if ((health <= 0 && !isMidAir))
        {
            myNavMeshAgent.speed = 0.0f;
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

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        UIDamageTextPool.Instance.ShowDamage(transform.position, CapsuleColliderHeight * 2.5f, damage);
        if(health < 0)
        {
            health = 0;
        }
        if (myState != CharacterState.Attack && health > 0)
        {
            myAnimator.Play("Damage");
        }
    }

    protected IEnumerator DeathBurrowDelay()
    {
        myCapsuleCollider.enabled = false;
        yield return new WaitForSeconds(3.0f);
        var time = 3.0f;

        while (true)
        {
            isBurrow = true;
            myNavMeshAgent.speed = 0.0f;
            myNavMeshAgent.enabled = false;
            transform.position += Vector3.down * Time.deltaTime * 0.2f;
            time -= Time.deltaTime;
            yield return null;
            if (time <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected void OnEnableSet()
    {
        isBurrow = false;
        myNavMeshAgent.enabled = true;
        myCapsuleCollider.enabled = true;
        myNavMeshAgent.updateRotation = true;
        myHealthBar.SetActive(true);

        if (PlayerControl.Instance != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = Constants.PlayerLayer;
        stateController.ChangeState(CharacterState.Move);

        myNavMeshAgent.speed = moveSpeed;
        myNavMeshAgent.acceleration = 50.0f;
    }

    protected void PropertySet()
    {
        var multiplier = GameManager.Instance.EnemyStatMultiplier;
        var gameLevel = GameManager.Instance.NowGameLevel;
        switch (myType)
        {
            case EnemyType.SlimeRabbit:
                health = 10 + Mathf.FloorToInt(gameLevel * 0.25f * 10);
                atkDamage = 4 + Mathf.FloorToInt(gameLevel * 0.25f * 4);
                moveSpeed = 4.0f * multiplier;
                atkSpeed = 1.0f * multiplier;
                break;
            case EnemyType.Mushroom:
                health = 15 + Mathf.FloorToInt(gameLevel * 0.25f * 15);
                atkDamage = 7 + Mathf.FloorToInt(gameLevel * 0.25f * 7);
                moveSpeed = 3.0f * multiplier;
                atkSpeed = 1.0f * multiplier;
                break;
            case EnemyType.Bee:
                health = 8 + Mathf.FloorToInt(gameLevel * 0.25f * 8);
                atkDamage = 5 + Mathf.FloorToInt(gameLevel * 0.25f * 5);
                moveSpeed = 2.0f * multiplier;
                atkSpeed = 1.0f * multiplier;
                break;
        }
        maxHealth = health;
        myAnimator.SetFloat("AttackSpeedMultiplier", atkSpeed);
    }
}
