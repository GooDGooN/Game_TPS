using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class RifleBulletTrail : MonoBehaviour
{
    private float maxDistance = 100.0f;
    private float speed = 150.0f;
    private bool isImpact;
    private Vector3 startPosition;
    private Vector3 hitPosition;
    private Vector3 hitNormal;
    private RifleBulletEffectManager bulletEffectManager;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void GetBulletEffectManager(RifleBulletEffectManager effectTarget)
    {
        bulletEffectManager = effectTarget;
    }

    private void OnEnable()
    {
        transform.LookAt(hitPosition);
        startPosition = transform.position;
        isImpact = true;
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        bool isBulletOverMaxDist = Vector3.Distance(startPosition, transform.position) > maxDistance;
        bool isBulletOverHitPos = Vector3.Distance(startPosition, transform.position) > Vector3.Distance(startPosition, hitPosition);
        if (isBulletOverMaxDist)
        {
            gameObject.SetActive(false);
        }
        if(isBulletOverHitPos) 
        {
            if(isImpact) 
            {
                bulletEffectManager.ActiveImpact(hitPosition, hitNormal);
            }
            gameObject.SetActive(false);
        }
    }

    public void SetHitPoint(Vector3 targetPos, Vector3 normal, bool impact = true)
    {
        hitPosition = targetPos;
        hitNormal = normal;
        isImpact = impact;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hitPosition, 0.1f);
    }*/
}
