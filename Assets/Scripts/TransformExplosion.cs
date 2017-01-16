using System.Collections;
using UnityEngine;

public class TransformExplosion : MonoBehaviour
{
    public int numParticles = 30;
    public float maxScale = 2.0f;
    public float minScale = 2.0f;
    public float maxTravelTime = 1.0f;
    public float minTravelTime = 0.5f;
    public float maxDistance = 10.0f;
    public float minDistance = 4.0f;
    public float fadeTime = 0.2f;
    public GameObject particlePrefab;
    public Color color;
    int numFinished = 0;

	void Start ()
    {
        for (int i = 0; i < numParticles; i++)
            CreateParticle();
	}

    private void CreateParticle()
    {
        var go = GameObject.Instantiate(particlePrefab, transform);
        go.transform.position = transform.position;
        go.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        var sr = go.GetComponent<SpriteRenderer>();
        sr.color = color;

        var travelDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        var travelDist = Random.Range(minDistance, maxDistance);
        var travelTime = Random.Range(minTravelTime, maxTravelTime);

        LeanTween.move(go, transform.position + (travelDir * travelDist), travelTime)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => ParticleFinished(go));
    }

    private void ParticleFinished(GameObject go)
    {
        var sr = go.GetComponent<SpriteRenderer>();
        LeanTween.value(go, (c) => sr.color = c, sr.color, new Color(sr.color.r, sr.color.g, sr.color.b, 0), fadeTime)
            .setOnComplete(() => GameObject.Destroy(go));

        numFinished++;
        if(numFinished >= numParticles)
            StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Destroy(gameObject);
    }
}
