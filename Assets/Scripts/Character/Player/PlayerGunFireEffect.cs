using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFireEffect : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
        transform.localEulerAngles = Vector3.up * -90.0f;
    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }
    private void Update()
    {
        if(!GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
