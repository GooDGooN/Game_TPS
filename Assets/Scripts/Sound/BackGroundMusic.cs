using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{

    [SerializeField] protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    protected virtual void Update()
    {
        audioSource.volume = GameSystem.GlobalMusicVolume;
        if (!audioSource.isPlaying && GameManager.IsGameStart)
        {
            audioSource.Play();
        }
    }

}
