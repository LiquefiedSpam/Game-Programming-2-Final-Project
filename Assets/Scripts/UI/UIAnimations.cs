using UnityEngine;
using System.Collections;

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
}
