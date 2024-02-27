using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool IsGameStart = false;
    public static int SurviveTime = 0;
    public static int KillCount = 0;
    public static int LongestSurviveTime = 0;
    public static int HighestKillCount = 0;
    public static float EnemyStatMultiplier = 1.0f;
    public static int NowGameLevel = 0;
    private static float levelupCountDown = 0.0f;

    private Coroutine timeCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        GetRecords();
        Instantiate();
    }

    private void Update()
    {
        if (IsGameStart)
        {
            GameLevelSet();
            if (timeCoroutine == null)
            {
                timeCoroutine = StartCoroutine(TimeFlow());
            }
        }
    }

    private IEnumerator TimeFlow()
    {
        float time = 0.0f;
        while(PlayerControl.Instance != null)
        {
            time += Time.deltaTime;
            if(time > 1.0f)
            {
                SurviveTime++;
                time = 0.0f;
            }
            yield return null;            
        }
    }

    private void Instantiate()
    {
        IsGameStart = false;
        SurviveTime = 0;
        KillCount = 0;
        LongestSurviveTime = 0;
        HighestKillCount = 0;
        EnemyStatMultiplier = 1.0f;
        NowGameLevel = 0;
        levelupCountDown = 0.0f;
}

    public void GetRecords()
    {
        LongestSurviveTime = PlayerPrefs.GetInt("LongestSurviveTime");
        HighestKillCount = PlayerPrefs.GetInt("HighestKillCount");
        if (LongestSurviveTime == 0)
        {
            PlayerPrefs.SetInt("LongestSurviveTime", 0);
        }
        if (HighestKillCount == 0)
        {
            PlayerPrefs.SetInt("HighestKillCount", 0);
        }
    }


    public Vector3 GetRandomSpawnPosition(bool isItem = false)
    {
        float minimumRange = isItem ? 7.5f : 10.0f;
        float maximumRange = isItem ? 12.5f : 20.0f;
        float range = maximumRange - minimumRange;
        var playerPosition = PlayerControl.Instance.transform.position;

        Vector3 spawnPos = new Vector3(Random.Range(-range, range), playerPosition.y + 50.0f, Random.Range(-range, range));
        spawnPos.x += spawnPos.x > 0 ? minimumRange : -minimumRange;
        spawnPos.z += spawnPos.z > 0 ? minimumRange : -minimumRange;
        spawnPos.x += playerPosition.x;
        spawnPos.z += playerPosition.z;

        if (Mathf.Abs(spawnPos.x) > 90.0f)
        {
            var sign = float.IsNegative(spawnPos.x) ? -1 : 1;
            spawnPos.x -= maximumRange * sign;
        }
        if (Mathf.Abs(spawnPos.z) > 90.0f)  
        {
            var sign = float.IsNegative(spawnPos.y) ? -1 : 1;
            spawnPos.z -= maximumRange * sign;
        }

        Physics.Raycast(spawnPos, Vector3.down, out var groundHit, float.PositiveInfinity, Constants.SolidLayer);

        spawnPos = groundHit.point + Vector3.up * 1.0f;

        return spawnPos;
    }

    public void GameLevelSet()
    {
        levelupCountDown += Time.deltaTime;
        if (levelupCountDown >= Constants.LevelUpTime)
        {
            levelupCountDown = 0.0f;
            NowGameLevel++;
            EnemyStatMultiplier = EnemyStatMultiplier >= 2.5f ? 2.5f : EnemyStatMultiplier + 0.1f;
        }
    }
}
