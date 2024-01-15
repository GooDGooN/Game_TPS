using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : Singleton<BulletContainer>
{
    [SerializeField] private GameObject hitScanBulletPrefab;
    private GameObject[] hitScanBullets = new  GameObject[100];
    private new void Awake()
    {
        base.Awake();
        for(int i = 0; i < hitScanBullets.Length; i++)
        {
            hitScanBullets[i] = Instantiate(hitScanBulletPrefab, transform);
            hitScanBullets[i].SetActive(false);
        }
    }

    public void BulletActive(Vector3 worldposition, BulletPoolType targetType)
    {
        foreach(GameObject bullet in hitScanBullets) 
        { 
            if(!bullet.activeSelf) 
            {
                bullet.SetActive(true);
                bullet.transform.position = worldposition;
                bullet.GetComponent<HitScanBullet>().SetType(targetType);
                break;
            }
        }
    }
}
