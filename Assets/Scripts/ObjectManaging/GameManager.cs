using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool IsGameStart = false;
    public static float SurviveTime = 0.0f;
    public static int KillCount = 0;
    public static float LongestSurviveTime = 0.0f;
    public static int HighestKillCount = 0;
    public static float EnemyStatMultiplier = 1.0f;
    public static int NowGameLevel = 0;
    private static float levelupCountDown = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        GetRecords();
    }

    private void Update()
    {
        if(IsGameStart)
        {
            GameLevelSet();
            SurviveTime += Time.deltaTime;
        }
    }

    public void GetRecords()
    {
        LongestSurviveTime = PlayerPrefs.GetFloat("LongestSurviveTime");
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
        float maximumRange = isItem ? 15.0f : 20.0f;
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
