using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProperty : CharacterProperty
{
    public LayerMask PlayerLayerMask { get => playerLayerMask; }
    [SerializeField] protected LayerMask playerLayerMask;

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
}
