using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    public void ActiveBullet(Vector3 position)
    {
        transform.position = position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((GlobalVarStorage.EnemyLayer & 1 << other.gameObject.layer) != 0)
        {
            if (other.gameObject.TryGetComponent<CharacterProperty>(out var result) && !other.isTrigger)
            {
                result.GetDamage(1);
                if (result.Health > 0)
                {
                    result.MyAnimator.Play("Damage");
                }
            }
        }
    }
    private void Update()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
