using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
/*    public static PlayerControl PlayerScript { get => playerScript; }
    private static PlayerControl playerScript;

    public static GameObject PlayerObject { get => playerObject; }
    private static GameObject playerObject;
*/

    public static LayerMask SolidLayer => LayerMask.GetMask("Solid");
    public static LayerMask PlayerLayer => LayerMask.GetMask("Player");
    public static LayerMask EnemyLayer => LayerMask.GetMask("Enemy");
}
