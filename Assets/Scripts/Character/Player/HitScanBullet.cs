using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBullet : MonoBehaviour
{
    private int myDamage;

    private List<Collider> testCollider = new List<Collider>();
    public void ActiveBullet(Vector3 position, int damage)
    {
        transform.position = position;
        myDamage = damage;
    }
    private void OnEnable()
    {
        testCollider.Clear();
        Debug.Log("Alive");
    }

    private void OnTriggerStay(Collider other)
    {
        testCollider.Add(other);
        Debug.Log("Contact");
        //Debug.Log($"contact : {other.gameObject}");
        if (!other.isTrigger && gameObject.activeSelf)
        {
            if ((GlobalVarStorage.EnemyLayer & (1 << other.gameObject.layer)) != 0)
            {
                if (other.gameObject.TryGetComponent<CharacterProperty>(out var resultObj))
                {
                    resultObj.GetDamage(myDamage);
                    UIDamageTextPool.Instance.ShowDamage(resultObj.transform.position, resultObj.CapsuleColliderHeight * 3.0f, myDamage);
                    if (resultObj.Health > 0)
                    {
                        resultObj.MyAnimator.Play("Damage");
                    }
                }
            }
        }
        Debug.Log(testCollider.Count);
    }

    private void Update()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}