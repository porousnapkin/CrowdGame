using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour {
    public SpriteRenderer visuals;
    public SpriteRenderer guide;
    const float visualScaleMultiple = 12.5f;
    public Vector2 rootPosition = Vector2.zero;
    public enum Orientation { Horizontal, Vertical }
    public Orientation orientation;
    public float bigSize = 100;
    public float smallSize = 2.5f;
    public float warningSize = 2.0f;
    public float lingerTime = 0.5f;
    public float warningTime = 1.0f;
    public float toFullGrowthTime = 0.2f;
    public float whiteTime = 0.1f;

    void Start()
    {
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

        SoundMaker.Instance.PlaySound("Laser");
        //WhiteFlash;
        var c = visuals.color;
        visuals.color = Color.white;
        yield return new WaitForSeconds(whiteTime);
        visuals.color = c;

        SetupLingerVisuals();
        yield return new WaitForSeconds(lingerTime);

        GameObject.Destroy(gameObject);
    }

    void SetupGuide()
    {
        guide.transform.position = rootPosition;
        guide.transform.localScale = CalculateKillSize() * visualScaleMultiple;
        guide.color = new Color(guide.color.r, guide.color.g, guide.color.b, 0.0f);

        LeanTween.value(gameObject, (c) => guide.color = c, guide.color, new Color(guide.color.r, guide.color.g, guide.color.b, 0.1f), 0.5f)
            .setEase(LeanTweenType.linear);
    }

    void SetupWarningVisuals()
    {
        visuals.transform.position = rootPosition;

        Vector2 startScale = CalculateWarningSize() * visualScaleMultiple;
        if (orientation == Orientation.Horizontal)
            startScale.y = 0;
        else
            startScale.x = 0;
        visuals.transform.localScale = startScale;
        var finalScale = CalculateWarningSize() * visualScaleMultiple;

        LeanTween.scale(visuals.gameObject, finalScale, warningTime)
            .setEase(LeanTweenType.easeInOutCubic);
        visuals.color = new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0.5f);
    }

    Vector2 CalculateWarningSize()
    {
        if (orientation == Orientation.Horizontal)
            return new Vector2(bigSize, warningSize);
        else
            return new Vector2(warningSize, bigSize);
    }

    void SetupGrowth()
    {
        var finalScale = CalculateKillSize() * visualScaleMultiple;
        LeanTween.scale(visuals.gameObject, finalScale, toFullGrowthTime)
            .setEase(LeanTweenType.linear);

        LeanTween.value(gameObject, (c) => visuals.color = c, visuals.color, new Color(visuals.color.r, visuals.color.g, visuals.color.b, 1), toFullGrowthTime)
            .setEase(LeanTweenType.linear);
    }

    void SetupFiredVisuals()
    {
        visuals.transform.position = rootPosition;
        visuals.transform.localScale = CalculateKillSize() * visualScaleMultiple;
        visuals.color = new Color(visuals.color.r, visuals.color.g, visuals.color.b, 1.0f);
    }

    Vector2 CalculateKillSize()
    {
        if (orientation == Orientation.Horizontal)
            return new Vector2(bigSize, smallSize);
        else
            return new Vector2(smallSize, bigSize);
    }

    void Kill()
    {
        var colliders = Physics2D.OverlapBoxAll(rootPosition, CalculateKillSize(), 0);
        for (int i = 0; i < colliders.Length; i++)
            HandleCollision(colliders[i]);
    }

    void HandleCollision(Collider2D other)
    {
        var crowder = other.gameObject.GetComponent<CrowdUnit>();
        if (crowder != null)
            crowder.Die();
    }

    void SetupLingerVisuals()
    {
        LeanTween.value(gameObject, (c) => visuals.color = c, visuals.color, new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0), lingerTime)
            .setEase(LeanTweenType.easeInOutCubic);
    }
}
