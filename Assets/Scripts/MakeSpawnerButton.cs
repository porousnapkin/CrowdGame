using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeSpawnerButton : MonoBehaviour {
    public CrowdCreator crowdCreator;
    public int cost = 20;
    public Color particleColor;
    public Text textCost;
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
        var newCost = CalcCost();
        textCost.text = "Spawner\n" + newCost.ToString();
        button.interactable = newCost <= crowdCreator.GetNumActive();
    }

    int CalcCost()
    {
        return (cost + crowdCreator.GetNumSpawners() * 5);
    }

    void CreateSpawner()
    {
        SoundMaker.Instance.PlaySound("Blip");
        crowdCreator.CombineTransformUnits(CalcCost(), (v) => crowdCreator.MakeUnitSpawner(v), particleColor, "FormSpawner");
    }
}
