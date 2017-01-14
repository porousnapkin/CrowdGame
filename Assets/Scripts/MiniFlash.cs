using System.Collections;
using UnityEngine;

public class MiniFlash : MonoBehaviour {
    public float maxWaitTime = 0.2f;
    public SpriteRenderer sprite;
    public float lifespan = 0.2f;
    public float maxSize = 2.0f;
    public float minSize = 0.5f;

	void Awake () {
        sprite.enabled = false;
        transform.localScale = Vector2.one * Random.Range(minSize, maxSize);
        StartCoroutine(Flash());
	}

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, maxWaitTime));
        sprite.enabled = true;
        LeanTween.value(gameObject, (c) => sprite.color = c, sprite.color, new Color(0, 0, 0, 0), lifespan)
            .setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(lifespan);
    }
}
