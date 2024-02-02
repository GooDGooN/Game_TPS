using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBulletEffectManager : MonoBehaviour
{
    private const int effectMaxAmount = 15;

    private GameObject[] bulletImpacts = new GameObject[effectMaxAmount];
    [SerializeField] private GameObject bulletSolidImpactPrefab;

    private void Start()
    {
        for (int i = 0; i < effectMaxAmount; i++)
        {
            bulletImpacts[i] = Instantiate(bulletSolidImpactPrefab, transform);
        }
    }

    public void ActiveImpact(Vector3 position, Vector3 normal)
    {
        foreach (var impact in bulletImpacts)
        {
            if (!impact.activeSelf)
            {
                impact.SetActive(true);
                impact.transform.position = position;
                impact.transform.LookAt(position + normal);
                break;
            }
        }
    }

}
