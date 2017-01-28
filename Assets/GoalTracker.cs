using UnityEngine;
using UnityEngine.UI;

public class GoalTracker : MonoBehaviour {
    public CrowdCreator crowd;
    public EnemySpawnManager spawnManager;
    public Text text;
    int numGold = 0;

	void Start () {
        crowd.GoldCreatedEvent += GoldCreated;
	}

    private void GoldCreated(CrowdUnit u)
    {
        u.DiedEvent += UnitDied;
        numGold++;
    }

    private void UnitDied(CrowdUnit u)
    {
        u.DiedEvent -= UnitDied;
        numGold--;
    }

    void Update () {
        text.text = "Goal: " + numGold + " / " + spawnManager.GetGoalGold();
	}
}
