using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip itemPickUpClip;
    public override void PlaySound(SoundType soundType, float pitch = 1.0f)
    {
        if(soundType == SoundType.ItemPickUp)
        {
            audioSource.PlayOneShot(itemPickUpClip);
        }
    }
}
