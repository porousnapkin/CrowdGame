using System.Collections.Generic;
using UnityEngine;

public class CrowdCreator : MonoBehaviour {
    public CrowdMoveData spawnerMoveData;
    public CrowdMoveData warriorMoveData;
    public CrowdMoveData goldMoveData;
    public GameObject crowderPrefab;
    public GameObject warriorDeathAnim;
    public GameObject warriorCicleKillAnim;
    public int numToMake = 1;
    List<CrowdUnit> units = new List<CrowdUnit>();
    List<CrowdUnit> specialUnits = new List<CrowdUnit>();
    public static Vector2 destination = Vector2.zero;
    public GameObject combineTransformAnim;
    public event System.Action<CrowdUnit> GoldCreatedEvent;
    public event System.Action LostEvent;
    bool lost = false;

	void Start () {
        destination = Vector2.zero;

        for (int i = 0; i < numToMake; i++)
            SpawnUnit(Vector3.zero);
	}

    public Vector2 GetPercievedCrowdCenter()
    {
        Vector3 pos = Vector3.zero;
        units.ForEach(u => pos += u.transform.position);
        specialUnits.ForEach(u => pos += u.transform.position);
        pos /= units.Count + specialUnits.Count;

        return pos;
    }

    public CrowdUnit SpawnUnit(Vector3 pos)
    {
        if (lost)
            return null;

        var go = GameObject.Instantiate(crowderPrefab, transform);
        go.transform.position = pos;
        var unit = go.GetComponent<CrowdUnit>();
        unit.destination = destination;
        unit.DiedEvent += UnitDied;
        units.Add(unit);
        return unit;
    }

    void UnitDied(CrowdUnit obj)
    {
        units.Remove(obj);
        specialUnits.Remove(obj);

        if (units.Count + specialUnits.Count <= 0)
        {
            lost = true;
            LostEvent();
        }
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

    public void MakeWarrior(Vector3 position)
    {
        var u = MakeSpecialUnit(position, warriorMoveData);
        var warrior = u.gameObject.AddComponent<Warrior>();
        warrior.killAnimation = warriorDeathAnim;
        warrior.killCircle = warriorCicleKillAnim;
    }

    public void MakeUnitSpawner(Vector3 position)
    {
        var u = MakeSpecialUnit(position, spawnerMoveData);
        var spawner = u.gameObject.AddComponent<CrowdSpawner>();
        spawner.creator = this;
    }

    public void MakeGoldUnit(Vector3 position)
    {
        var u = MakeSpecialUnit(position, goldMoveData);
        GoldCreatedEvent(u);
    }

    CrowdUnit MakeSpecialUnit(Vector3 position, CrowdMoveData moveData)
    {
        var u = SpawnUnit(position);
        units.Remove(u);
        specialUnits.Add(u);
        u.SetCrowdMoveData(moveData);
        PlaySpecialUnitMakeAnimation(u.gameObject);

        return u;
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

    public void CombineTransformUnits(int numUnits, System.Action<Vector3> callback, Color particleColor, string sound)
    {
        var transformingUnits = new List<CrowdUnit>();
        for (int i = 0; i < numUnits; i++)
            transformingUnits.Add(units[i]);

        units.RemoveRange(0, numUnits);

        var animGO = GameObject.Instantiate(combineTransformAnim, transform);
        var transformAnim = animGO.GetComponent<CombineTransformAnim>();
        transformAnim.units = transformingUnits;
        transformAnim.callback = callback;
        transformAnim.particleColor = particleColor;
        transformAnim.sound = sound;
    }

    public int GetNumSpawners()
    {
        return specialUnits.FindAll(s => s.gameObject.GetComponent<CrowdSpawner>() != null).Count;
    }
}

