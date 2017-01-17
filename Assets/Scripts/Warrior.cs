using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public float targettingSize = 20;
    public float attackMoveSpeed = 0.02f;
    public float killDistance = 1.1f;
    public GameObject killAnimation;
    public float decel = 0.03f;
    Vector3 velocity = Vector3.zero;
    bool isAttacking = false;
    CrowdUnit crowdAI;

    void Start()
    {
        crowdAI = GetComponent<CrowdUnit>();
    }

    void Update()
    {
        var targets = GetNearbyTargets();
        if (targets.Count > 0)
            UpdateAttacking(targets);
        else if (targets.Count == 0 && isAttacking)
            FinishAttacking();
    }

    private void UpdateAttacking(List<GameObject> targets)
    {
        if (!isAttacking)
            BeginAttacking();

        targets.Sort((a, b) => Mathf.RoundToInt(Vector3.Distance(a.transform.position, transform.position) - Vector3.Distance(b.transform.position, transform.position) * 100));
        var target = targets[0];
        var toTarget = target.transform.position - transform.position;
        if (toTarget.magnitude < killDistance)
            KillTarget(target);
        else
            MoveTowardsTarget(toTarget);
    }

    private void MoveTowardsTarget(Vector3 toTarget)
    {
        velocity -= velocity * decel;
        velocity += toTarget.normalized * attackMoveSpeed;
        transform.position += velocity;
    }

    private void BeginAttacking()
    {
        isAttacking = true;
        crowdAI.enabled = false;
        velocity = Vector3.zero;
    }

    private void KillTarget(GameObject target)
    {
        var killAnimationGO = GameObject.Instantiate(killAnimation, transform.parent);
        killAnimationGO.transform.position = target.transform.position;
        GameObject.Destroy(target);
        this.enabled = false;
        crowdAI.Die();
    }

    private void FinishAttacking()
    {
        isAttacking = false;
        crowdAI.enabled = true;
    }

    List<GameObject> GetNearbyTargets()
    {
        var overlapSphere = Physics2D.OverlapCircleAll(transform.position, targettingSize);
        var friends = overlapSphere.ToList().ConvertAll(c => c.gameObject);
        friends.RemoveAll(f => f == null || f.tag != "Enemy");
        return friends;
    }
}

