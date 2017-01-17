using UnityEngine;

public class Chaser : MonoBehaviour {
    public float speed = 0.1f;
    public float rotationSpeed = 0.1f;

    void Start()
    {
        var goalAngle = GetGoalAngle();
        transform.rotation = Quaternion.Euler(0, 0, goalAngle);
    }

    float GetGoalAngle()
    {
        var toDestination = CrowdCreator.destination - new Vector2(transform.position.x, transform.position.y);
        var goalAngle = Vector2.Angle(Vector2.up, toDestination.normalized);
        if (Vector3.Cross(Vector3.up, toDestination).z < 0)
            goalAngle = 360 - goalAngle;
        return goalAngle;
    }

	void Update () {
        var goalAngle = GetGoalAngle();
        var curAngle = transform.rotation.eulerAngles.z;
        if (curAngle > 270 && goalAngle < 90)
            goalAngle = 360 + goalAngle;
        else if (curAngle < 90 && goalAngle > 270)
            goalAngle = goalAngle - 360;

        var finalAngle = Mathf.Lerp(curAngle, goalAngle, rotationSpeed);
        var rotation = Quaternion.Euler(0, 0, finalAngle);
        transform.rotation = rotation;

        var forward = rotation * Vector3.up;

        transform.position += forward.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var cu = other.gameObject.GetComponent<CrowdUnit>();
        if (cu)
            cu.Die();
    }
}
