using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AdditionalCrowdUnits : MonoBehaviour {
    public int numToAdd = 25;
    public CrowdCreator crowdCreator;
    public GameObject textScalar;
    public Text text;
    public List<SpriteRenderer> sprites;
    public Color displayColor;
    public float fadeInTime = 0.3f;
    public float textFadeTime = 1.0f;
    public float textFinalScale = 0.1f;
    bool engaged = false;

	void Start () {
        text.text = "+" + numToAdd;

        var fadeInColor = new Color(displayColor.r, displayColor.g, displayColor.b, 0);
        text.color = fadeInColor;
        sprites.ForEach(s => s.color = fadeInColor);

        LeanTween.textColor(text.rectTransform, displayColor, fadeInTime);
        sprites.ForEach(s => LeanTween.alpha(s.gameObject, 0.3f, fadeInTime));
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (engaged)
            return;

        var cu = other.gameObject.GetComponent<CrowdUnit>();
        if (cu)
            Engage();
    }

    void Engage()
    {
        engaged = true;
        sprites.ForEach(s => LeanTween.alpha(s.gameObject, 0, fadeInTime));
        LeanTween.scale(textScalar, Vector3.one * textFinalScale, textFadeTime)
            .setEase(LeanTweenType.easeOutQuint);
        LeanTween.textColor(text.rectTransform, new Color(1, 1, 1, 0), textFadeTime)
            .setOnComplete(() => GameObject.Destroy(gameObject));

        for (int i = 0; i < numToAdd; i++)
            crowdCreator.SpawnUnit(transform.position);
    }
}
