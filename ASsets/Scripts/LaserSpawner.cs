using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour {
    public GameObject laserPrefab;
    public GameObject grenadePrefab;
    public int xRange = 20;
    public int yRange = 10;

	void Start () {
        StartCoroutine(LaserCoroutine());
	}
	
    IEnumerator LaserCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 3.0f));
            //CreateGrenade();
            CreateRandomLaser();
        }
    }

    void CreateRandomLaser()
    {
        var val = Random.value;
        if(val < 0.4f)
        {
            CreateHorizontalLaser();
        }
        else if (val < 0.8f)
        {
            CreateVerticalLaser();
        }
        else if(val < 0.95f)
        {
            CreateHorizontalLaser();
            CreateVerticalLaser();
        }
        else if(val < 0.97f)
        {
            CreateHorizontalLaser();
            CreateHorizontalLaser();
        }
        else
        {
            CreateVerticalLaser();
            CreateVerticalLaser();
        }
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

    void CreateGrenade()
    {
        var grenadeGO = GameObject.Instantiate(grenadePrefab, transform);
        var grenade = grenadeGO.GetComponent<Grenade>();
        grenade.rootPosition = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
    }
}
