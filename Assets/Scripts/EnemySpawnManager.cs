using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public enum SpawnType
    {
        Laser,
        Grenade,
        Chaser,
        AdditionalCrowdUnits,
        SuperLaser,
    }

    public EnemySpawner laserSpawner;
    public EnemySpawner grenadeSpawner;
    public EnemySpawner chaserSpawner;
    public EnemySpawner additionalUnitsSpawner;
    public EnemySpawner superLaserSpawner;
    public EnemySpawningSet activeSpawningSet;

    Dictionary<SpawnType, EnemySpawner> spawnTypeToSpawner = new Dictionary<SpawnType, EnemySpawner>();
    float intensityPercent = 0.0f;
    float levelTimer = 0;
    int goalAmount = 1;
    Coroutine runCoroutine;
    bool started = false;

    void Start()
    {
        spawnTypeToSpawner[SpawnType.Laser] = laserSpawner;
        spawnTypeToSpawner[SpawnType.Grenade] = grenadeSpawner;
        spawnTypeToSpawner[SpawnType.Chaser] = chaserSpawner;
        spawnTypeToSpawner[SpawnType.AdditionalCrowdUnits] = additionalUnitsSpawner;
        spawnTypeToSpawner[SpawnType.SuperLaser] = superLaserSpawner;

    }

    IEnumerator RunCoroutine()
    {
        while(true)
        {
            var list = activeSpawningSet.GetRandomSpawnDataList();
            foreach(var data in list)
            {
                if (Random.value < data.chanceToSkip)
                    continue;

                if (intensityPercent < data.intensityBeforeAppearing)
                    continue;

                var waitTime = Mathf.Lerp(data.maxLeadingTime, data.minLeadingTime, intensityPercent);
                yield return new WaitForSeconds(waitTime);
                data.whatToSpawn.ForEach(t => spawnTypeToSpawner[t].Spawn());
                waitTime = Mathf.Lerp(data.maxFollowingTime, data.minFollowingTime, intensityPercent);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    public void BeginLevel()
    {
        started = true;
        runCoroutine = StartCoroutine(RunCoroutine());
        levelTimer = 0;
    }

    public void SetUpNextLevel(EnemySpawningSet enemySpawningSet)
    {
        activeSpawningSet = enemySpawningSet;
        goalAmount += enemySpawningSet.goldGoal;
    }

    void Update()
    {
        if(started)
            levelTimer += Time.deltaTime;
        intensityPercent = levelTimer / activeSpawningSet.timeTillMaxIntensity;
        intensityPercent = Mathf.Min(1.0f, intensityPercent);
    }

    public float GetIntensity()
    {
        return intensityPercent;
    }

    public int GetGoalGold()
    {
        return goalAmount;
    }

    public void ClearEnemies()
    {
        foreach (Transform t in transform)
            GameObject.Destroy(t.gameObject);

        StopCoroutine(runCoroutine);
    }
}

