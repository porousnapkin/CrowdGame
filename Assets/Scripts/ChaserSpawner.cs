using UnityEngine;

public class ChaserSpawner : EnemySpawner
{
    public GameObject chaserPrefab;
    public int xExtent = 40;
    public int yExtent = 20;
    public float chanceVerticallyAligned = 0.65f;
    public CrowdCreator crowdCreator;

    public override void Spawn() {
        bool verticalAligned = Random.value < chanceVerticallyAligned;
        float x = verticalAligned? Random.Range(-xExtent, xExtent) : GetRandomExtent(xExtent);
        float y = verticalAligned? GetRandomExtent(yExtent) : Random.Range(-yExtent, yExtent);

        var chaserGO = GameObject.Instantiate(chaserPrefab, transform);
        chaserGO.transform.position = new Vector3(x, y, 0);

        chaserGO.GetComponent<Chaser>().crowdCreator = crowdCreator;
    }

    int GetRandomExtent(int extent)
    {
        if (Random.value > 0.5f)
            return extent;
        else
            return -extent;
    }
}

