using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimap : Singleton<UIMinimap>
{
    [SerializeField] private GameObject enemyIconPrefab;
    [SerializeField] private GameObject enemyIconStorage;
    [SerializeField] private Camera minimapCamera;
    private GameObject[] enemyIcons = new GameObject[Constants.EnemyObjAmout * Constants.EnemyTypeAmount];

    private void Start()
    {
        for(int i = 0; i< enemyIcons.Length; i++)
        {
            enemyIcons[i] = Instantiate(enemyIconPrefab, enemyIconStorage.transform);
            enemyIcons[i].SetActive(false);
            enemyIcons[i].GetComponent<UIEnemyMinimapIcon>().minimapCamera = minimapCamera;
        }
    }

    public void ActivateEnemyIcon(GameObject followTarget)
    {
        foreach(GameObject enemyIcon in enemyIcons)
        {
            if(!enemyIcon.activeSelf)
            {
                enemyIcon.GetComponent<UIEnemyMinimapIcon>().Initialize(followTarget);
                enemyIcon.SetActive(true);
                break;
            }
        }
    }
}
