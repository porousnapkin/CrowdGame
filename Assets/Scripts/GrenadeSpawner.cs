using UnityEngine;

public class GrenadeSpawner : EnemySpawner {
    public GameObject grenadePrefab;
    public int xRange = 20;
    public int yRange = 10;

    public override void Spawn() { 
        var grenadeGO = GameObject.Instantiate(grenadePrefab, transform);
        var grenade = grenadeGO.GetComponent<Grenade>();
        grenade.rootPosition = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
	}
}
