using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private float minimumSpawnRate = 0.2f;
    private float currentSpawnRate = 5.0f;
    private EnemyObjectPool enemyPool;

    protected override void Awake()
    {
        base.Awake();
        enemyPool = GetComponentInChildren<EnemyObjectPool>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        enemyPool = GetComponentInChildren<EnemyObjectPool>();
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(3.0f);
        int randomEnemySelectNum = 0;
        Transform playerTransform = PlayerControl.Instance.transform;
        while(PlayerControl.Instance != null)
        {
            
            randomEnemySelectNum = Random.Range(0, Constants.EnemyTypeAmount);

            float minimumRange = 7.5f;
            float maximumRange = 12.5f;
            float range = maximumRange - minimumRange;

            Vector3 randomPos = new Vector3(Random.Range(-range, range), playerTransform.position.y + 1.0f, Random.Range(-range, range));
            randomPos.x += randomPos.x > 0 ? minimumRange : -minimumRange;
            randomPos.z += randomPos.z > 0 ? minimumRange : -minimumRange;
            randomPos += playerTransform.position;

            enemyPool.SpawnEnemyByType((EnemyType)randomEnemySelectNum, randomPos);
            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    public void SpawnSplitSlimeRabbit(Vector3 position)
    {
        enemyPool.SpawnEnemyByType(EnemyType.SlimeRabbit, position, true);
    }
}
