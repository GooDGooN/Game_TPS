using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRifleAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip rifleFireClip;
    [SerializeField] private AudioClip rifleReloadClip;
    [SerializeField] private AudioClip rifleMagDryClip;
    public override void PlaySound(SoundType soundType, float pitch = 1.0f)
    {
        audioSource.pitch = pitch;
        switch (soundType)
        {
            case SoundType.RifleFire: audioSource.PlayOneShot(rifleFireClip); break;
            case SoundType.RifleReload: audioSource.PlayOneShot(rifleReloadClip); break;
            case SoundType.RifleMagDry: audioSource.PlayOneShot(rifleMagDryClip); break;
        }
    }
    public void CancelSound()
    {
        audioSource.Stop();
    }

}
