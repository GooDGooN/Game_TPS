using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SlimeRabbitControl : EnemyProperty
{
    public bool Jumping { get => jumping; set => jumping = value; }
    private bool jumping = false;
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;
    public bool IsSplit { get => isSplit; }
    private bool isSplit = false;

    public bool IsMidAir { get => isMidAir; }
    private bool isMidAir = false;

    private void Start()
    {
        PropertySet();
        if(GlobalVarStorage.Instance.PlayerObj != null)
        {
            myAnimator.SetBool("IsMove", true);
        }
        attackRangeCollider.includeLayers = playerLayerMask;
        stateController = new CharacterStateController(this);
        stateController.ChangeState(CharacterState.Move);

        MyNavMeshAgent.speed = moveSpeed;
        MyNavMeshAgent.acceleration = 50.0f;

        if(isSplit)
        {
            var vrandom = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            myRigidbody.AddForce((Vector3.up * 20.0f) + (vrandom * Random.Range(2.0f, 4.0f)));
            transform.localScale *= 0.75f;
        }
    }


    private void Update()
    {
        //Debug.Log($"Slime Rabbit health = {health}");
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            myCapsuleCollider.isTrigger = true;
            if (IsSplit)
            {
                stateController.ChangeState(CharacterState.Death);
                MyAnimator.SetTrigger("Death");
                StartCoroutine(DeathBurrowDelay());
            }
            else
            {
                for(int i = -1; i < 2; i += 2)
                {
                    var obj = Instantiate(this, transform.position, transform.rotation);
                    obj.isSplit = true;
                }
                Destroy(gameObject);
            }
        }
        if(isMidAir || jumping)
        {
            myNavMeshAgent.speed = moveSpeed;
        }
        else
        {
            myNavMeshAgent.speed = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        stateController.CurrentState.StateFixedUpdate();
        Physics.BoxCast(transform.position + new Vector3(0.0f, capsuleColliderHeight, 0.0f), new Vector3(0.4f, 0.01f, 0.4f), Vector3.down, out var info, Quaternion.identity, float.PositiveInfinity, solidLayer);
        Debug.Log(info.distance);
        if(info.distance != 0.0f && info.distance >= capsuleColliderHeight + 0.1f)
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

    private void PropertySet()
    {
        if(IsSplit)
        {
            myType = EnemyType.SlimeRabbit;
            health = 1;
            moveSpeed = 6.0f;
            atkDamage = 0;
            atkSpeed = 0;
        }
        else
        {
            myType = EnemyType.SlimeRabbit;
            health = 3;
            moveSpeed = 4.0f;
            atkDamage = 0;
            atkSpeed = 0;
        }
    }


    private IEnumerator DeathBurrowDelay()
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
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            myAnimator.SetBool("IsAttack", true);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if ((1 << other.gameObject.layer & playerLayerMask) != 0)
        {
            myAnimator.SetBool("IsAttack", false);
        }
    }

}
