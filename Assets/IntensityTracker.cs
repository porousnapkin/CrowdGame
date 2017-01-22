using UnityEngine.UI;
using UnityEngine;

public class IntensityTracker : MonoBehaviour {
    public EnemySpawnManager spawnManager;
    public Text text;

	void Update () {
        text.text = "Intensity " + Mathf.RoundToInt(spawnManager.GetIntensity() * 100) + "%";
	}
}
