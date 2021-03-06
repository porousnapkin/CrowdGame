using System.Collections.Generic;
using UnityEngine;

public class CombineTransformAnim : MonoBehaviour
{
    public List<CrowdUnit> units;
    public System.Action<Vector3> callback;
    int animsFinished = 0;
    Vector3 destination;
    public GameObject transformExplosionAnim;
    public Color particleColor;
    public string sound;

    public void Start()
    {
        destination = CalculateDestination();
        units.ForEach(u =>
        {
            var anim = u.gameObject.AddComponent<CombineTransformUnitAnim>();
            anim.destination = destination;
            anim.callback = AnimFinished;
        });
    }

    Vector3 CalculateDestination()
    {
        Vector3 position = Vector3.zero;
        units.ForEach(u => position += u.transform.position);
        position /= units.Count;
        return position;
    }

    void AnimFinished()
    {
        animsFinished++;
        if(animsFinished >= units.Count)
        {
            SoundMaker.Instance.PlaySound(sound);
            var explosionGO = GameObject.Instantiate(transformExplosionAnim, transform.parent);
            explosionGO.transform.position = destination;
            explosionGO.GetComponent<TransformExplosion>().color = particleColor;

            callback(destination);
            GameObject.Destroy(this);
            units.ForEach(u => GameObject.Destroy(u.gameObject));
        }
    }
}

