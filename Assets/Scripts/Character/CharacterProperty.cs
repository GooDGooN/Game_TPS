using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    public int MaxHealth { get => maxHealth; }
    protected int maxHealth;
    public int Health { get => health; }
    protected int health;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    protected float moveSpeed;
    public int AtkDamage { get => atkDamage; }
    protected int atkDamage;
    public float AtkSpeed { get => atkSpeed; }
    protected float atkSpeed;
    public float JumpPower { get => jumpPower; }
    protected float jumpPower = 40.0f;

    public float CapsuleColliderHeight { get => capsuleColliderHeight; }
    protected float capsuleColliderHeight;
    protected float capsuleColliderRadius;

    #region FSM
    public CharacterState MyState { get => myState; set => myState = value; }
    protected CharacterState myState = CharacterState.Idle;

    public CharacterUpperState MyUpperState { get => myUpperState; set => myUpperState = value; }
    protected CharacterUpperState myUpperState = CharacterUpperState.Normal;

    protected CharacterStateController stateController;
    #endregion


    protected virtual void Awake()
    {
        capsuleColliderHeight = myCapsuleCollider.height * 0.5f;
        capsuleColliderRadius = myCapsuleCollider.radius;
    }

    public Rigidbody MyRigidbody { get => myRigidbody; }
    protected Rigidbody myRigidbody
    {
        get
        {
            var comp = GetComponent<Rigidbody>();
            if (comp == null)
            {
                comp = GetComponentInChildren<Rigidbody>();
            }
            return comp;
        }
    }

    public Animator MyAnimator { get => myAnimator; }
    protected Animator myAnimator
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

    public CapsuleCollider MyCapsuleCollider { get => myCapsuleCollider; }
    protected CapsuleCollider myCapsuleCollider
    {
        get
        {
            var comp = GetComponent<CapsuleCollider>(); 
            if(comp == null)
            {
                comp = GetComponentInChildren<CapsuleCollider>();
            }
            return comp;
        }
    }


    public void GetDamage(int value) => health -= value;

    public void GetHeal(int value) => health += value;

}
