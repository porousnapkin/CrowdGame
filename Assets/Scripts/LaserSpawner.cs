using System.Collections;
using UnityEngine;

public class LaserSpawner : EnemySpawner {
    public GameObject laserPrefab;
    public int xRange = 20;
    public int yRange = 10;
    public float chanceForHorizontalLaser = 0.4f;

    public override void Spawn()
    {
        var val = Random.value;
        if(val < chanceForHorizontalLaser)
            CreateHorizontalLaser();
        else 
            CreateVerticalLaser();
    }

    void CreateHorizontalLaser()
    {
        var laserGO = GameObject.Instantiate(laserPrefab, transform);
        var laser = laserGO.GetComponent<Laser>();
        laser.orientation = Laser.Orientation.Horizontal;
        laser.rootPosition = new Vector2(0, Random.Range(-yRange, yRange));
    }

    void CreateVerticalLaser()
    {
        var laserGO = GameObject.Instantiate(laserPrefab, transform);
        var laser = laserGO.GetComponent<Laser>();
        laser.orientation = Laser.Orientation.Vertical;
        laser.rootPosition = new Vector2(Random.Range(-xRange, xRange), 0);
    }
}
