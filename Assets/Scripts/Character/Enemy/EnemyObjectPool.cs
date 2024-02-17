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

    public void SpawnEnemyByType(EnemyType enemyType, Vector3 pos, bool isSkillActive = false)
    {
        GameObject[] pool = null;
        switch (enemyType)
        {
            case EnemyType.SlimeRabbit: pool = slimeRabbitPool; break;
            case EnemyType.Mushroom: pool = mushroomPool; break;
            case EnemyType.Bee: pool = beePool; break;
        }

        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
            {
                if (enemyType == EnemyType.SlimeRabbit)
                {
                    obj.GetComponent<SlimeRabbitControl>().IsSplit = isSkillActive;
                }
                obj.transform.position = pos;
                obj.SetActive(true);
                UIMinimap.Instance.ActivateEnemyIcon(obj);
                break;
            }
        }
    }
}
