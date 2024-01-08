using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    private SphereCollider myCollider;
    private GlobalEnums.BulletPoolType myType = GlobalEnums.BulletPoolType.None; 


    private void OnTriggerEnter(Collider other)
    {
        /*myType = GlobalEnums.BulletPoolType.None;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);*/
    }

    public void SetType(GlobalEnums.BulletPoolType targetType) => myType = targetType;
}
