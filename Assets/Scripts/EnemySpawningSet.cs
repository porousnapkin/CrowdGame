using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningSet : ScriptableObject
{
    [System.Serializable]
    public class SpawnData
    {
        public List<EnemySpawnManager.SpawnType> whatToSpawn;
        public float maxLeadingTime = 1.0f;
        public float minLeadingTime = 1.0f;
        public float maxFollowingTime = 1.0f;
        public float minFollowingTime = 1.0f;
        public int instancesPerSet = 1;
    }

    public List<SpawnData> spawnData = new List<SpawnData>();
    public float timeTillMaxIntensity = 45.0f;

    public List<SpawnData> GetRandomSpawnDataList()
    {
        var newShuffledList = new List<SpawnData>();
        spawnData.ForEach(s =>
        {
            for (int i = 0; i < s.instancesPerSet; i++)
                newShuffledList.Add(s);
        });
        newShuffledList.Sort((a, b) => Random.Range(-100, 100));

        return newShuffledList;
    }
}

