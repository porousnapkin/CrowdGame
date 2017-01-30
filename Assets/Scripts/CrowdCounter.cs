using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CrowdCounter : MonoBehaviour {
    public CrowdCreator crowdManager;
    Text text;

	void Start () {
        text = GetComponent<Text>();
	}
	
	void Update () {
        text.text = crowdManager.GetNumActive().ToString();
	}
}
