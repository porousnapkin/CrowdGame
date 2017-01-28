using UnityEngine;

public class AdditionalCrowdUnitsPickupSpawner : EnemySpawner
{
    public GameObject additionalCrowdUnitsPrefab;
    public int xRange = 20;
    public int yRange = 10;
    public CrowdCreator crowdCreator;

    public override void Spawn()
    {
        var additionalGO = GameObject.Instantiate(additionalCrowdUnitsPrefab, transform);
        additionalGO.transform.position = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));

        additionalGO.GetComponent<AdditionalCrowdUnits>().crowdCreator = crowdCreator;
    }
}

