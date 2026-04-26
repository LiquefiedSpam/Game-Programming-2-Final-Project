using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public static class UIAnimations
{
    public static IEnumerator ScaleTo(Transform t, Vector3 targetScale, float duration)
    {
        Vector3 startScale = t.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            yield return null;
        }

        t.localScale = targetScale;
    }

    public static IEnumerator PopAndShrink(Transform t, Vector3 defaultScale, float popAmt)
    {
        yield return ScaleTo(t, defaultScale * popAmt, 0.1f);
        yield return ScaleTo(t, defaultScale, 0.2f);
    }

    public static IEnumerator FloatUpAndFade(TextMeshProUGUI text, float distance = 50f, float duration = 1f)
    {
        Vector3 startPos = text.rectTransform.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0f, distance, 0f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            text.rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        Object.Destroy(text.gameObject);
    }

    public static IEnumerator FloatUpAndFade(SpriteRenderer sprite, float distance = 1.5f, float duration = 1f)
    {
        Vector3 startPos = sprite.transform.position;
        Vector3 endPos = startPos + new Vector3(Random.Range(-0.3f, 0.3f), distance, 0f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            sprite.transform.forward = Camera.main.transform.forward;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            sprite.transform.position = Vector3.Lerp(startPos, endPos, t);
            sprite.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        Object.Destroy(sprite.gameObject);
    }

    public static IEnumerator LerpAlpha(Image img, float fromAlpha = 1, float toAlpha = 0, float duration = 1)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Color c = img.color;
            c.a = Mathf.Lerp(fromAlpha, toAlpha, t);
            img.color = c;

            yield return null;
        }

        Color final = img.color;
        final.a = toAlpha;
        img.color = final;
    }
}