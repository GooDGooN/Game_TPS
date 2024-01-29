using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : Singleton<EnemyObjectPool>
{
    private const int objAmout = 30;

    [SerializeField] private GameObject slimeRabbitObj;
    private GameObject[] slimeRabbitPool = new GameObject[objAmout];
    [SerializeField] private GameObject mushroomObj;
    private GameObject[] mushroomPool = new GameObject[objAmout];
    [SerializeField] private GameObject beeObj;
    private GameObject[] beePool = new GameObject[objAmout];

    private void Start()
    {
        for(int i = 0; i < objAmout; i++)
        {
            slimeRabbitPool[i] = Instantiate(slimeRabbitObj, transform);
            slimeRabbitPool[i].SetActive(false);
            mushroomPool[i] = Instantiate(mushroomObj, transform);
            mushroomPool[i].SetActive(false);
            beePool[i] = Instantiate(beeObj, transform);
            beePool[i].SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            beeSpawnTest(new Vector3(0.0f, 2.0f, 0.0f));
        }
    }

    public void SlimeRabbitSpawnTest(Vector3 pos, bool isSplit = false)
    {
        foreach (var obj in slimeRabbitPool)
        {
            if(!obj.activeSelf)
            {
                obj.GetComponent<SlimeRabbitControl>().IsSplit = isSplit;
                obj.SetActive(true);
                obj.transform.position = pos;
                break;
            }
        }
    }
    public void MushroomSpawnTest(Vector3 pos)
    {
        foreach (var obj in mushroomPool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                obj.transform.position = pos;
                break;
            }
        }
    }
    public void beeSpawnTest(Vector3 pos)
    {
        foreach (var obj in beePool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                obj.transform.position = pos;
                break;
            }
        }
    }
}
