using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip damageClip;
    public override void PlaySound(SoundType soundType, float pitch = 1.0f)
    {
        if (soundType == SoundType.Damage)
        {
            pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(damageClip);
        }
        audioSource.pitch = pitch;
    }
}
