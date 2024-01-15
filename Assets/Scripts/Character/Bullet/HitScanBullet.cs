using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    private SphereCollider myCollider;
    private BulletPoolType myType = BulletPoolType.None; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<SlimeRabbitControl>(out var result))
        {
            result.GetDamage(1);
        }
        myType = BulletPoolType.None;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void SetType(BulletPoolType targetType) => myType = targetType;
}
