using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public float targettingSize = 25;
    public float attackMoveSpeed = 0.02f;
    public float killDistance = 1.1f;
    public float killCircleRadius = 10.0f;
    public GameObject killAnimation;
    public GameObject killCircle;
    public float decel = 0.045f;
    GameObject target;
    Vector3 velocity = Vector3.zero;
    bool isAttacking = false;
    CrowdUnit crowdAI;

    void Start()
    {
        crowdAI = GetComponent<CrowdUnit>();
    }

    void Update()
    {
        var targets = GetNearbyTargets(targettingSize);
        if (targets.Count > 0 || target != null)
            UpdateAttacking(targets);
        else if (targets.Count == 0 && isAttacking)
            FinishAttacking();
    }

    private void UpdateAttacking(List<GameObject> targets)
    {
        if (!isAttacking)
            BeginAttacking();

        if (target == null)
            PickTarget(targets);
        
        var toTarget = target.transform.position - transform.position;
        if (toTarget.magnitude < killDistance)
            KillTarget(target);
        else
            MoveTowardsTarget(toTarget);
    }

    private void PickTarget(List<GameObject> targets)
    {
        targets.Sort((a, b) => Mathf.RoundToInt(Vector3.Distance(a.transform.position, transform.position) - Vector3.Distance(b.transform.position, transform.position) * 100));
        target = targets[0];
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
        target = null;
    }

    private void KillTarget(GameObject target)
    {
        var killAnimationGO = GameObject.Instantiate(killAnimation, transform.parent);
        killAnimationGO.transform.position = target.transform.position;
        var killCircleGO = GameObject.Instantiate(killCircle, transform.parent);
        killCircleGO.transform.position = target.transform.position;
        var kc = killCircleGO.GetComponent<KillCircle>();
        kc.radius = killCircleRadius;
        SoundMaker.Instance.PlaySound("WarriorBoom");

        GameObject.Destroy(target);
        var targets = GetNearbyTargets(killCircleRadius);
        targets.ForEach(t => GameObject.Destroy(t));

        this.enabled = false;
        crowdAI.Die();
    }

    private void FinishAttacking()
    {
        isAttacking = false;
        crowdAI.enabled = true;
    }

    List<GameObject> GetNearbyTargets(float radius)
    {
        var overlapSphere = Physics2D.OverlapCircleAll(transform.position, radius);
        var targets = overlapSphere.ToList().ConvertAll(c => c.gameObject);
        targets.RemoveAll(f => f == null || f.tag != "Enemy");
        return targets;
    }
}

