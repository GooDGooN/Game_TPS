using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    public int Health { get => health; }
    protected int health = 100;

    protected float BlendPosX = 0.0f;
    protected float BlendPosXVelocity = 0.0f;
    protected float BlendPosY = 0.0f;
    protected float BlendPosYVelocity = 0.0f;
    protected float BlendPosSmoothTime = 0.1f;

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
}
