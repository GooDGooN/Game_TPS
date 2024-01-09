using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    public int Health { get => health; set => health = value; }
    protected int health;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    protected float moveSpeed;
    public int AtkDamage { get => atkDamage; set => atkDamage = value; }
    protected int atkDamage;
    public float AtkSpeed { get => atkSpeed; set => atkSpeed = value; }
    protected float atkSpeed;
    public float JumpPower { get => jumpPower; }
    protected float jumpPower = 40.0f;


    public Rigidbody MyRigidbody { get => myRigidbody; }
    protected Rigidbody myRigidbody;

    public Animator MyAnimator { get => myanimator; }
    protected Animator myanimator
    {
        get
        { 
            var comp = GetComponent<Animator>();
            if(comp == null)
            {
                comp = GetComponentInChildren<Animator>();
            }
            return comp;
        }
    }

    public void GetDamage(int value) => health -= value;

}
