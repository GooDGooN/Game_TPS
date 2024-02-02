using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBulletImpact : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }
    private void Update()
    {
        if (!GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
