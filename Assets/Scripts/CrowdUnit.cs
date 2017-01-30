using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class CrowdUnit : MonoBehaviour {
    public CrowdMoveData data;
    public SpriteRenderer spriteRenderer;
    public Collider2D col;
    public Vector3 destination = Vector3.zero;
    Vector3 speed = Vector3.zero;
    public float maxDelayBeforeMove = 1.0f;
    public event System.Action<CrowdUnit> DiedEvent = delegate { };

    public void SetCrowdMoveData(CrowdMoveData data)
    {
        this.data = data;
        spriteRenderer.color = data.color;
    }

	void Update () {
        ProcessMovement();
	}

    void ProcessMovement()
    {
        var separationVector = CalculateSeparationVector();
        var destinationVector = CalculateDestinationVector();

        speed -= speed * data.speedDecay;
        speed += separationVector + destinationVector;
        transform.position += speed;
    }

    public void MoveDestination(Vector3 newDestination)
    {
        StartCoroutine(DelayMoveRandomly(newDestination));
    }

    public IEnumerator DelayMoveRandomly(Vector3 newDestination)
    {
        yield return new WaitForSeconds(Random.Range(0.0f, maxDelayBeforeMove));
        this.destination = newDestination;
    }

    Vector3 CalculateDestinationVector()
    {
        var toDestination = (destination - transform.position);
        if (toDestination.magnitude > data.destinationMaxDistanceStrength)
            toDestination = toDestination.normalized * data.destinationMaxDistanceStrength;
        return toDestination * data.destinationStrength;
    }

    Vector3 CalculateSeparationVector()
    {
        Vector3 separation = Vector3.zero;
        var friends = GetNearbyFriends();
        friends.ForEach(f => separation += CalculateFriendsSeparation(f));

        return separation;
    }

    private Vector3 CalculateFriendsSeparation(GameObject f)
    {
        var toFriend = f.transform.position - transform.position;
        var force = Mathf.Max(data.separationStrength - (toFriend.magnitude * data.separationFalloffSpeed), 0);
        if (toFriend == Vector3.zero)
            toFriend = new Vector3(Random.value, Random.value);
        return -toFriend.normalized * force;
    }

    List<GameObject> GetNearbyFriends()
    {
        var overlapSphere = Physics2D.OverlapCircleAll(transform.position, data.sphereCastSize);
        var friends = overlapSphere.ToList().ConvertAll(c => c.gameObject);
        friends.RemoveAll(f => f == null || f.tag != "Crowd");
        return friends;
    }

    public void Die()
    {
        DiedEvent(this);

        spriteRenderer.color = Color.red;
        col.enabled = false;
        enabled = false;
        LeanTween.value(gameObject, (c) => spriteRenderer.color = c, spriteRenderer.color, new Color(1.0f, 0, 0, 0), 1.0f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => GameObject.Destroy(gameObject));
    }
}
