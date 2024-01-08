using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVarStorage : Singleton<UIVarStorage>
{
    public PlayerControl PlayerScript { get => playerScript; }
    [SerializeField] private PlayerControl playerScript;

    public int PlayerHP { get => playerScript.Health; }
    public GameObject PlayerFirePointObj { get => playerScript.RifleMuzzle; }
}
