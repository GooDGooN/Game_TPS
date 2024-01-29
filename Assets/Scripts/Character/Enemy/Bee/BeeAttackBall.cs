using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttackBall : MonoBehaviour
{
    private int damage;
    private Vector3 smoothDampVelocity = Vector3.zero;
    private float speed = 0;
    private Vector3 startPoint;
    private Transform firePoint;


    public void Initialize(int damage, Transform firePoint)
    {
        this.damage = damage;
        startPoint = transform.position;
        this.firePoint = firePoint;
    }

    public void Fire()
    {
        speed = 10.0f;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Vector3.Distance(transform.localScale, Vector3.one) > Mathf.Epsilon)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.one * 0.5f, ref smoothDampVelocity, 0.3f);
        }
        else
        {
            transform.localScale = Vector3.one * 0.5f;
        }

        if(speed == 0.0f)
        {
            transform.position = firePoint.position;
        }

        if(Vector3.Distance(startPoint, transform.position) > 10.0f)
        {
            Debug.Log("out");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var layer = GlobalVarStorage.Instance.PlayerLayer | GlobalVarStorage.Instance.SolidLayer;
        Debug.Log(other.gameObject.layer);
        if ((layer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.gameObject.TryGetComponent<CharacterProperty>(out var result))
            {
                result.GetDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
