using UnityEngine.UI;
using UnityEngine;


public class NextLevelBeginToast : MonoBehaviour
{
    public float time = 1.0f;
    public Text text;
    public float startScale = 1.0f;
    public float endScale = 2.0f;
    public Color startColor;

    public void RunToast(System.Action callback)
    {
        text.color = startColor;
        text.transform.localScale = Vector3.one * startScale;
        LeanTween.textAlpha(text.rectTransform, 0, time);
        LeanTween.scale(text.gameObject, Vector3.one * endScale, time)
            .setOnComplete(() => callback());
    }
}

