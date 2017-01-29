using UnityEngine;
using UnityEngine.UI;

public class GoalTracker : MonoBehaviour {
    public CrowdCreator crowd;
    public EnemySpawnManager spawnManager;
    public Text text;
    public event System.Action levelWon = delegate { };
    int numGold = 0;

	void Start () {
        crowd.GoldCreatedEvent += GoldCreated;
	}

    private void GoldCreated(CrowdUnit u)
    {
        u.DiedEvent += UnitDied;
        numGold++;

        if (spawnManager.GetGoalGold() <= numGold)
            levelWon();
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
