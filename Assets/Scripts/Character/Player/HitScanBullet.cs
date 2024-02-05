using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    private int myDamage;
    public void ActiveBullet(Vector3 position, int damage)
    {
        transform.position = position;
        myDamage = damage;
    }
    private void OnTriggerStay(Collider other)
    {
        if(!other.isTrigger)
        {
            if ((GlobalVarStorage.EnemyLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.gameObject.TryGetComponent<CharacterProperty>(out var result))
                {
                    result.GetDamage(myDamage);
                    UIDamageTextPool.Instance.ShowDamage(result.transform.position + Vector3.up * 1.5f, myDamage);
                    if (result.Health > 0)
                    {
                        result.MyAnimator.Play("Damage");
                    }
                }
            }
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
