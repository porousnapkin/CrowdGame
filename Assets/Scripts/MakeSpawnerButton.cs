using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeSpawnerButton : MonoBehaviour {
    public CrowdCreator crowdCreator;
    public int cost = 20;
    public Color particleColor;
    Button button;

	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateSpawner);
	}

    void OnDestroy()
    {
        button.onClick.RemoveListener(CreateSpawner);
    }

    void Update()
    {
        button.interactable = cost <= crowdCreator.GetNumActive();
    }

    void CreateSpawner()
    {
        crowdCreator.CombineTransformUnits(cost, (v) => crowdCreator.MakeUnitSpawner(v), particleColor);
    }
}
