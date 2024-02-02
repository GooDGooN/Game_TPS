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
        MyNavMeshAgent.updateRotation = false;
        transform.position += Vector3.up * height;
        myType = EnemyType.Bee;
        PropertySet();
        if (GlobalVarStorage.PlayerObject != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = GlobalVarStorage.PlayerLayer;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        MyNavMeshAgent.speed = moveSpeed;
        MyNavMeshAgent.acceleration = 50.0f;
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
            attackBall.transform.LookAt(GlobalVarStorage.PlayerObject.transform.position);
            attackBall.GetComponent<BeeAttackBall>().Fire();
        }
    }
}
