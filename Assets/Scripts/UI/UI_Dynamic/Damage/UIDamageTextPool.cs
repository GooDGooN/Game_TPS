using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageTextPool : Singleton<UIDamageTextPool>
{
    private GameObject[] damageTexts = new GameObject[50];
    [SerializeField] private GameObject damageTextPrefab;
    private void Start()
    {
        for(int i = 0; i < damageTexts.Length; i++)
        {
            damageTexts[i] = Instantiate(damageTextPrefab, transform);
            damageTexts[i].SetActive(false);
        }
    }

    public void ShowDamage(Vector3 position, int damage)
    {
        foreach(GameObject obj in damageTexts)
        {
            if(!obj.activeSelf) 
            {
                obj.GetComponent<TMP_Text>().text = damage.ToString();
                obj.transform.position = position;
                obj.SetActive(true);
                break;
            }
        }
    }
}
