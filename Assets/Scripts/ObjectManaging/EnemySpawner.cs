using CharacterNamespace;
using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private float minimumSpawnRate = 0.2f;
    private float currentSpawnRate = 5.0f;
    private EnemyObjectPool enemyPool;
    private Coroutine startCoroutine;

    protected override void Awake()
    {
        base.Awake();
        enemyPool = GetComponentInChildren<EnemyObjectPool>();
    }

    private void Update()
    {
        if (GameManager.IsGameStart && startCoroutine == null)
        {
            startCoroutine = StartCoroutine(Spawning());
        }
    }

   
    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(3.0f);
        while(PlayerControl.Instance != null)
        {
            var randomEnemySelectNum = Random.Range(0, Constants.EnemyTypeAmount);
            var randomPos =  GameManager.Instance.GetRandomSpawnPosition();

            if(enemyPool.CountActiveEnemy() < 30)
            {
                enemyPool.SpawnEnemyByType((EnemyType)randomEnemySelectNum, randomPos);
            }

            currentSpawnRate = 5.0f - GameManager.NowGameLevel * 0.2f;
            currentSpawnRate = Mathf.Clamp(currentSpawnRate,  minimumSpawnRate, currentSpawnRate);

            yield return new WaitForSeconds(currentSpawnRate);
        }
    }

    public void SpawnSplitSlimeRabbit(Vector3 position)
    {
        enemyPool.SpawnEnemyByType(EnemyType.SlimeRabbit, position, true);
    }
}
