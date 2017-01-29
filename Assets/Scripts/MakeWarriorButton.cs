using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeWarriorButton : MonoBehaviour
{
    public CrowdCreator crowdCreator;
    public int cost = 30;
    public Color particleColor;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateWarrior);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(CreateWarrior);
    }

    void Update()
    {
        button.interactable = cost <= crowdCreator.GetNumActive();
    }

    void CreateWarrior()
    {
        SoundMaker.Instance.PlaySound("Blip");
        crowdCreator.CombineTransformUnits(cost, (v) => crowdCreator.MakeWarrior(v), particleColor, "FormWarrior");
    }
}

