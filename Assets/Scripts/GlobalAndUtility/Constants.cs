using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    public static LayerMask SolidLayer => LayerMask.GetMask("Solid");
    public static LayerMask PlayerLayer => LayerMask.GetMask("Player");
    public static LayerMask EnemyLayer => LayerMask.GetMask("Enemy");
    public static LayerMask TreeLayer => LayerMask.GetMask("Tree");

    public const int EnemyObjAmout = 40;

    public const int EnemyTypeAmount = 3;

    public const float LevelUpTime = 30.0f;
}
