using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAnimatorControl : MonoBehaviour
{
    private MushroomControl parent;
    private void Awake()
    {
        parent = GetComponentInParent<MushroomControl>();
    }
    public void MushroomAttack()
    {
        if(parent.MyState == CharacterState.Attack)
        {
            parent.Attack = true;
        }
    }
}
