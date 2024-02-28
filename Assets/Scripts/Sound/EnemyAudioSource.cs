using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip[] damageClips = new AudioClip[3];

    public override void PlaySound(SoundType soundType, float pitch = 1.0f)
    {
        if (soundType == SoundType.Damage)
        {
            audioSource.PlayOneShot(damageClips[Random.Range(0, 3)]);
        }
    }
}
