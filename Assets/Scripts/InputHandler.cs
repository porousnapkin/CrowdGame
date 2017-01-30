using UnityEngine;

public class InputHandler : MonoBehaviour {
    public CrowdCreator handle;
    public Vector3 inputOffset;
    public static int numBlockers = 0;

	void Update () {
        if (Input.GetMouseButtonDown(0) && numBlockers <= 0)
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;
            handle.MoveCrowdDestination(pz + inputOffset);
        }
	}
}
