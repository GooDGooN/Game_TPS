using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarStorage : Singleton<GlobalVarStorage>
{
    public static PlayerControl PlayerScript { get => playerScript; }
    private static PlayerControl playerScript;

    public static GameObject PlayerObject { get => playerObject; }
    private static GameObject playerObject;

    public static LayerMask SolidLayer { get => solidLayer; }
    private static LayerMask solidLayer;
    public static LayerMask PlayerLayer { get => playerLayer; }
    private static LayerMask playerLayer;
    public static LayerMask EnemyLayer { get => enemyLayer; }
    private static LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        solidLayer = LayerMask.GetMask("Solid");
        playerLayer = LayerMask.GetMask("Player");
        enemyLayer = LayerMask.GetMask("Enemy");
        playerScript = FindObjectOfType<PlayerControl>();
        playerObject = playerScript.gameObject;
    }
}
