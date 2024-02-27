using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip[] damage = new AudioClip[3];

    public override void PlaySound(SoundType soundType)
    {
        audioSource.volume = GameSystem.Instance.GlobalSoundVolume;
        if (soundType == SoundType.Damage)
        {
            audioSource.PlayOneShot(damage[Random.Range(0, 3)]);
        }
    }
}
