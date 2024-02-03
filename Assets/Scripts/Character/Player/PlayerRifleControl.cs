using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class PlayerRifleControl : MonoBehaviour
{
    public int MaxImumMagazineCapacity;
    public int CurrentMagazineCapacity { get => currentMagazineCapacity; }
    private int currentMagazineCapacity;
    [SerializeField] private GameObject rifleMuzzle;

    #region RAYCAST
    private RaycastHit[] castHits = new RaycastHit[50];
    private float maxHitDist = 100.0f;
    #endregion

    #region BULLET
    private const int effectMaxAmount = 15;
    private GameObject[] bulletTrails = new GameObject[effectMaxAmount];
    [SerializeField] private GameObject bulletTrailPrefab;

    private GameObject[] gunFireEffects = new GameObject[effectMaxAmount];
    [SerializeField] private GameObject fireEffectPrefab;

    [SerializeField] private GameObject bulletPool;
    [SerializeField] private RifleBulletEffectManager bulletEffectManager;
    #endregion


    private void Start()
    {
        for (int i = 0; i < effectMaxAmount; i++)
        {
            gunFireEffects[i] = Instantiate(fireEffectPrefab, rifleMuzzle.transform);
            bulletTrails[i] = Instantiate(bulletTrailPrefab, bulletPool.transform);
            bulletTrails[i].GetComponent<RifleBulletTrail>().GetBulletEffectManager(bulletEffectManager);
        }
    }

    public void BulletFire(Vector3 targetPoint)
    {
        var muzzleTransform = rifleMuzzle.transform;
        foreach (var effect in gunFireEffects)
        {
            if (!effect.activeSelf)
            {
                effect.SetActive(true);
                effect.transform.position = muzzleTransform.position;
                break;
            }
        }

        // This will active and set the hitpoint of bullet
        foreach (var trail in bulletTrails)
        {
            if (!trail.activeSelf)
            {
                var direction = (targetPoint - muzzleTransform.position).normalized;
                var rayHits =  Physics.RaycastAll(muzzleTransform.position, direction, float.PositiveInfinity);
                
                RaycastHit seletedHit = new RaycastHit();
                if (rayHits.Length > 0)
                {
                    seletedHit = rayHits[0];
                    foreach (var hit in rayHits)
                    {
                        if (seletedHit.distance > hit.distance && !hit.collider.isTrigger)
                        {
                            seletedHit = hit;
                        }
                    }
                }

                if(seletedHit.collider != null)
                {
                    trail.GetComponent<RifleBulletTrail>().SetHitPoint(seletedHit.point, seletedHit.normal);
                }
                else
                {
                    var pos = muzzleTransform.forward * maxHitDist + muzzleTransform.position;
                    trail.GetComponent<RifleBulletTrail>().SetHitPoint(pos, Vector3.zero, false);
                }
                trail.transform.position = muzzleTransform.position;
                trail.SetActive(true);
                break;
            }
        }
    }

}
