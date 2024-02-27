using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundPlayer<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    public abstract void PlaySound(SoundType soundType);
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
