using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundPlayer<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    public abstract void PlaySound(SoundType soundType, float pitch = 1.0f);
    protected virtual void Update()
    {
        audioSource.volume = GameSystem.Instance.GlobalSoundVolume;
    }
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
