using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRifleAudioSource : SoundPlayer<MonoBehaviour>
{
    [SerializeField] private AudioClip rifleFire;
    [SerializeField] private AudioClip rifleReload;
    [SerializeField] private AudioClip rifleMagDry;
    public override void PlaySound(SoundType soundType)
    {
        audioSource.volume = GameSystem.Instance.GlobalSoundVolume;
        switch (soundType)
        {
            case SoundType.RifleFire: audioSource.PlayOneShot(rifleFire); break;
            case SoundType.RifleReload: audioSource.PlayOneShot(rifleReload); break;
            case SoundType.RifleMagDry: audioSource.PlayOneShot(rifleMagDry); break;
        }
    }
    public void CancelSound()
    {
        audioSource.Stop();
    }

}
