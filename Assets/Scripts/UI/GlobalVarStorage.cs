using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarStorage : Singleton<GlobalVarStorage>
{
    public PlayerControl PlayerScript { get => playerScript; }
    [SerializeField] private PlayerControl playerScript;
    public GameObject PlayerObj { get => playerObj; }
    [SerializeField] private GameObject playerObj;

    public LayerMask SolidLayer { get => solidLayer; }
    protected LayerMask solidLayer;
    public LayerMask PlayerLayer { get => playerLayer; }
    protected LayerMask playerLayer;
    public LayerMask EnemyLayer { get => enemyLayer; }
    protected LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        solidLayer = LayerMask.GetMask("Solid");
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");

    }
}
