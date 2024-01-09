using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    private SphereCollider myCollider;
    private GlobalEnums.BulletPoolType myType = GlobalEnums.BulletPoolType.None; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyControl>(out var result))
        {
            result.GetDamage(1);
        }
        Debug.Log("s");
        myType = GlobalEnums.BulletPoolType.None;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void SetType(GlobalEnums.BulletPoolType targetType) => myType = targetType;
}
