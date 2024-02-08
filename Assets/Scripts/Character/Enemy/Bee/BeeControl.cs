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

    private float height = 0.0f; // 2.0f

    private void OnEnable()
    {
        myCapsuleCollider.enabled = true;
        myNavMeshAgent.updateRotation = true;
        transform.position += Vector3.up * height;
        myType = EnemyType.Bee;
        PropertySet();
        if (PlayerControl.Instance != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = Constants.PlayerLayer;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        myNavMeshAgent.speed = moveSpeed;
        myNavMeshAgent.acceleration = 50.0f;
    }

    protected override void FixedUpdate()
    {
        transform.position -= Vector3.up * height;
        base.FixedUpdate();
    }

    private void Update()
    {        
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            myCapsuleCollider.enabled = false;
            myNavMeshAgent.updateRotation = false;
            MyAnimator.SetTrigger("Death");
            StartCoroutine(DeathBurrowDelay());
        }
        transform.position += Vector3.up * height;
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
