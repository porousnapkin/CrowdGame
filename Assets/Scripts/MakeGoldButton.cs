using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeGoldButton : MonoBehaviour
{
    public CrowdCreator crowdCreator;
    public int cost = 20;
    public Color particleColor;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CreateGold);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(CreateGold);
    }

    void Update()
    {
        button.interactable = cost <= crowdCreator.GetNumActive();
    }

    void CreateGold()
    {
        crowdCreator.CombineTransformUnits(cost, (v) => crowdCreator.MakeGoldUnit(v), particleColor);
    }
}

