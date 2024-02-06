using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageTextPool : Singleton<UIDamageTextPool>
{
    private GameObject[] damageTexts = new GameObject[50];
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Canvas dynamicCanvas;
    private void Start()
    {
        for(int i = 0; i < damageTexts.Length; i++)
        {
            damageTexts[i] = Instantiate(damageTextPrefab, transform);
            damageTexts[i].SetActive(false);
        }
    }

    public void ShowDamage(Vector3 position, float yDeltaPos, int damage)
    {
        foreach(GameObject obj in damageTexts)
        {
            if(!obj.activeSelf) 
            {
                var randomxDeltaPos = Random.Range(-1.0f, 1.0f);
                var deltaPos = (Vector3.up * yDeltaPos) + (Vector3.right * randomxDeltaPos);
                obj.GetComponent<TMP_Text>().text = damage.ToString();
                obj.transform.position = position;
                obj.GetComponent<UIDamageText>().targetPosition = position + deltaPos;
                obj.SetActive(true);
                break;
            }
        }
    }
}
