using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    public static LayerMask SolidLayer => LayerMask.GetMask("Solid");
    public static LayerMask PlayerLayer => LayerMask.GetMask("Player");
    public static LayerMask EnemyLayer => LayerMask.GetMask("Enemy");

    public const int EnemyObjAmout = 30;

    public const int EnemyTypeAmount = 3;

    public const float LevelUpTime = 20.0f;

}
