using System;
using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour {
    public SpriteRenderer visuals;
    public SpriteRenderer guide;
    public Vector2 rootPosition = Vector2.zero;
    public float maxRadius = 10.0f;
    public float warningRadius = 1.0f;
    public float warningTime = 1.0f;
    public float toFullGrowthTime = 1.0f;
    public float lingerTime = 1.0f;
    public float whiteTime = 0.1f;
    public GameObject miniFlash;
    public float miniFlashRange = 5.0f;
    public int minMiniFlashes = 3;
    public int maxMiniFlashes = 6;

    void Start () {
        StartCoroutine(Lifespan());
	}

    IEnumerator Lifespan()
    {
        SetupGuide();
        SetupWarningVisuals();
        yield return new WaitForSeconds(warningTime);

        SetupGrowth();
        yield return new WaitForSeconds(toFullGrowthTime);

        SetupFiredVisuals();
        Kill();

        CreateMiniFlashes();

        SoundMaker.Instance.PlaySound("Grenade");
        //WhiteFlash;
        var c = visuals.color;
        visuals.color = Color.white;
        yield return new WaitForSeconds(whiteTime);
        visuals.color = c;

        SetupLingerVisuals();
        yield return new WaitForSeconds(lingerTime);

        GameObject.Destroy(gameObject);
    }

    private void SetupGuide()
    {
        guide.transform.localScale = Vector2.one * maxRadius * 2;
        guide.transform.position = rootPosition;
        guide.color = new Color(guide.color.r, guide.color.g, guide.color.b, 0.0f);

        LeanTween.value(gameObject, (c) => guide.color = c, guide.color, new Color(guide.color.r, guide.color.g, guide.color.b, 0.1f), 0.5f)
            .setEase(LeanTweenType.linear);
    }

    private void SetupWarningVisuals()
    {
        visuals.transform.localScale = Vector2.zero * 2;
        visuals.transform.position = rootPosition;
        var finalScale = Vector2.one * warningRadius;
        LeanTween.scale(visuals.gameObject, finalScale, warningTime)
            .setEase(LeanTweenType.easeInOutCubic);
        visuals.color = new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0.5f);
    }

    private void SetupGrowth()
    {
        var finalScale = Vector2.one * maxRadius;
        LeanTween.scale(visuals.gameObject, finalScale, toFullGrowthTime)
            .setEase(LeanTweenType.linear);

        LeanTween.value(gameObject, (c) => visuals.color = c, visuals.color, new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0.8f), toFullGrowthTime)
            .setEase(LeanTweenType.linear);
    }

    private void SetupFiredVisuals()
    {
        visuals.transform.position = rootPosition;
        visuals.transform.localScale = Vector2.one * maxRadius * 2;
        visuals.color = new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0.8f);
        guide.enabled = false;
    }

    private void Kill()
    {
        var colliders = Physics2D.OverlapCircleAll(rootPosition, maxRadius);
        for (int i = 0; i < colliders.Length; i++)
            HandleCollision(colliders[i]);
    }

    void HandleCollision(Collider2D other)
    {
        var crowder = other.gameObject.GetComponent<CrowdUnit>();
        if (crowder != null)
            crowder.Die();
    }

    private void SetupLingerVisuals()
    {
        LeanTween.value(gameObject, (c) => visuals.color = c, visuals.color, new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0), lingerTime)
            .setEase(LeanTweenType.easeInOutCubic);
    }

    private void CreateMiniFlashes()
    {
        var num = UnityEngine.Random.Range(minMiniFlashes, maxMiniFlashes);
        for(int i = 0; i < num; i++)
        {
            var flashGO = GameObject.Instantiate(miniFlash, transform);
            var pos = new Vector2(UnityEngine.Random.Range(-miniFlashRange, miniFlashRange), UnityEngine.Random.Range(-miniFlashRange, miniFlashRange)) + rootPosition;
            flashGO.transform.position = pos;
        }
    }
}
