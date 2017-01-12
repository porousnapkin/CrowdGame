using System.Collections.Generic;
using UnityEngine;

public class CrowdCreator : MonoBehaviour {
    public CrowdMoveData spawnerMovData;
    public GameObject crowderPrefab;
    public int numToMake = 5;
    List<CrowdUnit> units = new List<CrowdUnit>();
    public static Vector2 destination = Vector2.zero;
    

	void Start () {
        for (int i = 0; i < numToMake; i++)
            SpawnUnit(Vector3.zero);

        MakeRandomUnitSpawner();
        MakeRandomUnitSpawner();
        MakeRandomUnitSpawner();
	}

    public void SpawnUnit(Vector3 pos)
    {
        var go = GameObject.Instantiate(crowderPrefab, transform);
        go.transform.position = pos;
        var unit = go.GetComponent<CrowdUnit>();
        unit.destination = destination;
        unit.DiedEvent += UnitDied;
        units.Add(unit);
    }

    private void UnitDied(CrowdUnit obj)
    {
        units.Remove(obj);
    }

    public void MoveCrowdDestination(Vector3 newDestination)
    {
        destination = newDestination;
        units.ForEach(u => u.MoveDestination(newDestination));
    }

    public int GetNumActive()
    {
        return units.Count;
    }

    public void MakeRandomUnitSpawner()
    {
        var u = units[Random.Range(0, units.Count)];
        var spawner = u.gameObject.AddComponent<CrowdSpawner>();
        spawner.creator = this;
        u.SetCrowdMoveData(spawnerMovData);
    }
}
