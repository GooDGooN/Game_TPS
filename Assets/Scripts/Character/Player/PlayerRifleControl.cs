using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class PlayerRifleControl : MonoBehaviour
{
    public int MaxImumMagazineCapacity { get => maxImumMagazineCapacity; }
    private int maxImumMagazineCapacity;
    public int CurrentMagazineCapacity { get => currentMagazineCapacity; }
    private int currentMagazineCapacity;
    [SerializeField] private GameObject rifleMuzzle;

    #region RAYCAST
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
        maxImumMagazineCapacity = currentMagazineCapacity = 20;
        for (int i = 0; i < effectMaxAmount; i++)
        {
            gunFireEffects[i] = Instantiate(fireEffectPrefab, rifleMuzzle.transform);
            bulletTrails[i] = Instantiate(bulletTrailPrefab, bulletPool.transform);
            bulletTrails[i].GetComponent<RifleBulletTrail>().GetBulletEffectManager(bulletEffectManager);
        }
    }

    public void BulletFire(Vector3 targetPoint, int damageValue)
    {
        if(currentMagazineCapacity > 0)
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
            Vector3 hitPoint = Vector3.zero;
            foreach (var trail in bulletTrails)
            {
                if (!trail.activeSelf)
                {
                    var direction = (targetPoint - muzzleTransform.position).normalized;
                    var rayHits = Physics.RaycastAll(muzzleTransform.position, direction, float.PositiveInfinity);

                    RaycastHit selectedHit = new RaycastHit();
                    if (rayHits.Length > 0)
                    {
                        selectedHit = rayHits[0];
                        foreach (var hit in rayHits)
                        {
                            if (selectedHit.distance > hit.distance && !hit.collider.isTrigger)
                            {
                                selectedHit = hit;
                            }
                        }
                    }

                    if (selectedHit.collider != null)
                    {
                        hitPoint = selectedHit.point;
                        trail.GetComponent<RifleBulletTrail>().SetHitPoint(hitPoint, selectedHit.normal);

                        var selectedHitObj = selectedHit.collider.gameObject;
                        
                        // Enemy get damage
                        if ((Constants.EnemyLayer & (1 << selectedHitObj.layer)) != 0)
                        {
                            if (selectedHitObj.TryGetComponent<CharacterProperty>(out var resultObj))
                            {
                                resultObj.GetDamage(damageValue);
                                UIDamageTextPool.Instance.ShowDamage(resultObj.transform.position, resultObj.CapsuleColliderHeight * 2.5f, damageValue);
                                if (resultObj.Health > 0)
                                {
                                    resultObj.MyAnimator.Play("Damage");
                                }
                            }
                        }

                        //HitScanBullet.GetComponent<HitScanBullet>().ActiveBullet(hitPoint, damageValue);
                        //HitScanBullet.SetActive(true);
                    }
                    else
                    {
                        hitPoint = muzzleTransform.forward * maxHitDist + muzzleTransform.position;
                        trail.GetComponent<RifleBulletTrail>().SetHitPoint(hitPoint, Vector3.zero, false);
                    }
                    trail.transform.position = muzzleTransform.position;
                    trail.SetActive(true);
                    break;
                }
            }
            currentMagazineCapacity--;
        }
    }

    public void ReloadMagazine() => currentMagazineCapacity = maxImumMagazineCapacity;

}
