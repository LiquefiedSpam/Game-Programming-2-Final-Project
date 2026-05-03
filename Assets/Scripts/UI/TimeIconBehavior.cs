using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TimeIconBehavior : MonoBehaviour
{
    [SerializeField] private Image image;
    private float transitionDuration = .5f;

    private Coroutine transition;

    public static TimeIconBehavior Ins => _instance;
    private static TimeIconBehavior _instance;

    public bool IsTransitioning => transition != null;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(this); return; }
        _instance = this;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
        image.color = Color.white;
        transform.localRotation = Quaternion.identity;
    }

    public void TransitionTo(Sprite newSprite)
    {
        if (transition != null) StopCoroutine(transition);
        transition = StartCoroutine(DoTransition(newSprite));
    }

    private IEnumerator DoTransition(Sprite newSprite)
    {
        float elapsed = 0f;

        // Fade/rotate out
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, -20, t));
            image.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        // Swap sprite while invisible
        image.sprite = newSprite;
        transform.localRotation = Quaternion.Euler(0, 0, 20);
        elapsed = 0f;

        // Fade/rotate in
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(20, 0, t));
            image.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        SetSprite(newSprite);

        yield return UIAnimations.PopAndShrink(image.transform, image.transform.localScale, 1.2f);
        transition = null;
    }
}
