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
}
