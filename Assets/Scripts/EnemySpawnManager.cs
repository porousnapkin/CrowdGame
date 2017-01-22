using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public enum SpawnType
    {
        Laser,
        Grenade,
        Chaser
    }

    public EnemySpawner laserSpawner;
    public EnemySpawner grenadeSpawner;
    public EnemySpawner chaserSpawner;
    public EnemySpawningSet activeSpawningSet;

    Dictionary<SpawnType, EnemySpawner> spawnTypeToSpawner = new Dictionary<SpawnType, EnemySpawner>();
    float intensityPercent = 0.0f;
    float levelTimer = 0;

    void Start()
    {
        spawnTypeToSpawner[SpawnType.Laser] = laserSpawner;
        spawnTypeToSpawner[SpawnType.Grenade] = grenadeSpawner;
        spawnTypeToSpawner[SpawnType.Chaser] = chaserSpawner;

        StartCoroutine(RunCoroutine());
    }

    IEnumerator RunCoroutine()
    {
        while(true)
        {
            var list = activeSpawningSet.GetRandomSpawnDataList();
            foreach(var data in list)
            {
                var waitTime = Mathf.Lerp(data.maxLeadingTime, data.minLeadingTime, intensityPercent);
                yield return new WaitForSeconds(waitTime);
                data.whatToSpawn.ForEach(t => spawnTypeToSpawner[t].Spawn());
                waitTime = Mathf.Lerp(data.maxFollowingTime, data.minFollowingTime, intensityPercent);
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
        intensityPercent = levelTimer / activeSpawningSet.timeTillMaxIntensity;
        intensityPercent = Mathf.Min(1.0f, intensityPercent);
    }

    public float GetIntensity()
    {
        return intensityPercent;
    }
}

