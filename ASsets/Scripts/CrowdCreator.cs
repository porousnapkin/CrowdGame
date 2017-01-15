using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCreator : MonoBehaviour {
    public CrowdMoveData spawnerMovData;
    public GameObject crowderPrefab;
    public int numToMake = 5;
    List<CrowdUnit> units = new List<CrowdUnit>();
    List<CrowdUnit> specialUnits = new List<CrowdUnit>();
    public static Vector2 destination = Vector2.zero;

	void Start () {
        for (int i = 0; i < numToMake; i++)
            SpawnUnit(Vector3.zero);

        StartCoroutine(TestCoroutine());
	}

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(4);
        CombineTransformUnits(10, (v) => MakeUnitSpawner(v) );
        yield return new WaitForSeconds(4);
        CombineTransformUnits(10, (v) => MakeUnitSpawner(v) );
        yield return new WaitForSeconds(4);
        CombineTransformUnits(10, (v) => MakeUnitSpawner(v) );
    }

    public CrowdUnit SpawnUnit(Vector3 pos)
    {
        var go = GameObject.Instantiate(crowderPrefab, transform);
        go.transform.position = pos;
        var unit = go.GetComponent<CrowdUnit>();
        unit.destination = destination;
        unit.DiedEvent += UnitDied;
        units.Add(unit);
        return unit;
    }

    private void UnitDied(CrowdUnit obj)
    {
        units.Remove(obj);
    }

    public void MoveCrowdDestination(Vector3 newDestination)
    {
        destination = newDestination;
        units.ForEach(u => u.MoveDestination(newDestination));
        specialUnits.ForEach(u => u.MoveDestination(newDestination));
    }

    public int GetNumActive()
    {
        return units.Count;
    }

    public void MakeUnitSpawner(Vector3 position)
    {
        var u = SpawnUnit(position);
        units.Remove(u);
        specialUnits.Add(u);
        var spawner = u.gameObject.AddComponent<CrowdSpawner>();
        spawner.creator = this;
        u.SetCrowdMoveData(spawnerMovData);
        PlaySpecialUnitMakeAnimation(u.gameObject);
    }

    void PlaySpecialUnitMakeAnimation(GameObject go)
    {
        var scale = go.transform.localScale;
        LeanTween.scale(go, scale * 3, 0.1f)
            .setOnComplete(() => LeanTween.scale(go, scale, 0.3f)
                .setEase(LeanTweenType.easeInQuad));
        var sr = go.GetComponent<SpriteRenderer>();
        var color = sr.color;
        sr.color = Color.white;
        LeanTween.value(go, (c) => sr.color = c, Color.white, color, 0.3f)
            .setEase(LeanTweenType.easeInQuad);
    }

    public void CombineTransformUnits(int numUnits, System.Action<Vector3> callback)
    {
        var transformingUnits = new List<CrowdUnit>();
        for (int i = 0; i < numUnits; i++)
            transformingUnits.Add(units[i]);

        units.RemoveRange(0, numUnits);

        var transformAnim = gameObject.AddComponent<CombineTransformAnim>();
        transformAnim.units = transformingUnits;
        transformAnim.callback = callback;
    }
}

