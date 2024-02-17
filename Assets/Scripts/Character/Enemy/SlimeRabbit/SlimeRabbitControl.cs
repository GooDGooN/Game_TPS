using CharacterNamespace;
using UnityEngine;

public class SlimeRabbitControl : EnemyProperty
{
    public bool Jumping { get => jumping; set => jumping = value; }
    private bool jumping = false;
    public bool Attack { get => attack; set => attack = value; }
    private bool attack = false;
    public bool IsSplit { get => isSplit; set => isSplit = value; }
    private bool isSplit = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        myType = EnemyType.SlimeRabbit;
        if (isSplit)
        {
            health /= 2;
            atkSpeed *= 2;
        }
        if(isSplit)
        {
            transform.localScale = Vector3.one * 0.75f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnDisable()
    {
        isSplit = false;
    }


    private void Update()
    {
        stateController.CurrentState.StateUpdate();
        if (health <= 0)
        {
            if (IsSplit)
            {
                if (!MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                {
                    MyAnimator.SetTrigger("Death");
                    StartCoroutine(DeathBurrowDelay());
                }
            }
            else
            {
                for(int i = -1; i < 2; i += 2)
                {
                    EnemySpawner.Instance.SpawnSplitSlimeRabbit(transform.position);
                }
                gameObject.SetActive(false);
            }
        }
        if(isMidAir || (jumping && health > 0))
        {
            myNavMeshAgent.speed = moveSpeed;
        }
        else
        {
            myNavMeshAgent.speed = 0.0f;
        }
    }
}
