using UnityEngine;

public class PlayerRifleControl : MonoBehaviour
{
    public int MaxImumMagazineCapacity;
    public int CurrentMagazineCapacity;
    [SerializeField] private GameObject rifleMuzzle;
    #region RAYCAST
    private float maxHitDist = 30.0f;
    #endregion

    [Header("Bullet")]
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
        MaxImumMagazineCapacity = CurrentMagazineCapacity = 20;
        for (int i = 0; i < effectMaxAmount; i++)
        {
            gunFireEffects[i] = Instantiate(fireEffectPrefab, rifleMuzzle.transform);
            bulletTrails[i] = Instantiate(bulletTrailPrefab, bulletPool.transform);
            bulletTrails[i].GetComponent<RifleBulletTrail>().GetBulletEffectManager(bulletEffectManager);
        }
    }

    public void BulletFire(Vector3 targetPoint, int damageValue)
    {
        if(CurrentMagazineCapacity > 0)
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
                    
                    trail.transform.position = muzzleTransform.position + (muzzleTransform.forward * 0.5f);

                    if (selectedHit.collider != null && selectedHit.distance < maxHitDist)
                    {
                        hitPoint = selectedHit.point;
                        trail.GetComponent<RifleBulletTrail>().SetHitPoint(hitPoint, selectedHit.normal);

                        var selectedHitObj = selectedHit.collider.gameObject;
                        
                        // Enemy get damage
                        if ((Constants.EnemyLayer & (1 << selectedHitObj.layer)) != 0)
                        {
                            if (selectedHitObj.TryGetComponent<CharacterProperty>(out var resultObj))
                            {
                                var distanceMultiply = Mathf.InverseLerp(maxHitDist, maxHitDist * 0.3f, selectedHit.distance);
                                damageValue = Mathf.CeilToInt(damageValue * distanceMultiply);
                                resultObj.GetDamage(damageValue);
                            }
                        }
                    }
                    else
                    {
                        trail.GetComponent<RifleBulletTrail>().SetHitPoint(targetPoint, Vector3.zero, false);
                    }
                    trail.SetActive(true);
                    break;
                }
            }
            CurrentMagazineCapacity--;
        }
    }

    public void ReloadMagazine()
    {
        CurrentMagazineCapacity = MaxImumMagazineCapacity;
    }
}
