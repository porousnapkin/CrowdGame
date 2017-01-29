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
        additionalGO.transform.position = GetLocation();

        additionalGO.GetComponent<AdditionalCrowdUnits>().crowdCreator = crowdCreator;
    }

    public Vector2 GetLocation()
    {
        var crowdCenter = crowdCreator.GetPercievedCrowdCenter();
        Vector2 pos = Vector2.one;
        for(int i = 0; i < 10; i++)
        {
            pos = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
            if (Vector2.Distance(crowdCenter, pos) > 10)
                break;
        }

        return pos;
    }
}

