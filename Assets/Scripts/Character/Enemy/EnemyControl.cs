using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterProperty
{
    private enum EnemyType
    {
        SlimeRabbit,
        Bee,
        Mushroom,
        Bat,
        Mummy,
    }
    [SerializeField] private EnemyType myType;

    private void Awake()
    {
        switch(myType)
        {
            case EnemyType.SlimeRabbit:
                health = 500;
                moveSpeed = 0;
                atkDamage = 0;
                atkSpeed = 0;
                break;
        }
    }

    private void Update()
    {
        Debug.Log($"Slime Rabbit health = {health}");
    }
}
