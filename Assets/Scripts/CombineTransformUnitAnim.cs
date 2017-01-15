using UnityEngine;


public class CombineTransformUnitAnim : MonoBehaviour
{
    public Vector3 destination;
    public System.Action callback;
    public float speed = 0.1f;

    void Start()
    {
        GetComponent<CrowdUnit>().enabled = false;
    }

    void Update()
    {
        var toDestination = destination - transform.position;
        if(toDestination.magnitude < speed)
        {
            transform.position = destination;
            callback();
        }
        else
        {
            transform.position += toDestination.normalized * speed;
        }
    }
}

