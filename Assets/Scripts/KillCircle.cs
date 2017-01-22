using System.Collections;
using UnityEngine;

public class KillCircle : MonoBehaviour
{
    public float radius;
    public SpriteRenderer visuals;
    public float whiteTime;
    public float lingerTime;

    void Start()
    {
        StartCoroutine(Lifespan());
    }

    IEnumerator Lifespan()
    {
        SetupFiredVisuals();

        //WhiteFlash;
        var c = visuals.color;
        visuals.color = Color.white;
        yield return new WaitForSeconds(whiteTime);
        visuals.color = c;

        SetupLingerVisuals();
        yield return new WaitForSeconds(lingerTime);

        GameObject.Destroy(gameObject);
    }

    private void SetupFiredVisuals()
    {
        visuals.transform.position = transform.position;
        visuals.transform.localScale = Vector2.one * radius * 2;
        visuals.color = new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0.8f);
    }

    private void SetupLingerVisuals()
    {
        LeanTween.value(gameObject, (c) => visuals.color = c, visuals.color, new Color(visuals.color.r, visuals.color.g, visuals.color.b, 0), lingerTime)
            .setEase(LeanTweenType.easeInOutCubic);
    }
}