using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject slimeRabbitObj;
    private GameObject[] slimeRabbitPool = new GameObject[Constants.EnemyObjAmout];
    [SerializeField] private GameObject mushroomObj;
    private GameObject[] mushroomPool = new GameObject[Constants.EnemyObjAmout];
    [SerializeField] private GameObject beeObj;
    private GameObject[] beePool = new GameObject[Constants.EnemyObjAmout];


    private void Start()
    {
        for(int i = 0; i < Constants.EnemyObjAmout; i++)
        {
            slimeRabbitPool[i] = Instantiate(slimeRabbitObj, transform);
            slimeRabbitPool[i].SetActive(false);
            mushroomPool[i] = Instantiate(mushroomObj, transform);
            mushroomPool[i].SetActive(false);
            beePool[i] = Instantiate(beeObj, transform);
            beePool[i].SetActive(false);
        }
    }

    public void SpawnSlimeRabbit(Vector3 pos, bool isSplit = false)
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
    public void SpawnMushroom(Vector3 pos)
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
    public void SpawnBee(Vector3 pos)
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
