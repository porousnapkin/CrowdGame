using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour {
    public UnityEngine.UI.Button button;
	void Start () {
        button.onClick.AddListener(Reset);
	}

    void Reset()
    {
        button.onClick.RemoveListener(Reset);
        SceneManager.LoadScene(0);
    }
}
