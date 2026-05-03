using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class TimeSliderGroup : MonoBehaviour
{
    [SerializeField] public Slider frontSlider;
    [SerializeField] public Slider middleSlider;
    [SerializeField] public Slider backSlider;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text unitsText;

    private RectTransform _rect;
    private Vector2 _defaultAnchoredPos;
    private Vector2 _defaultSizeDelta;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _defaultAnchoredPos = _rect.anchoredPosition;
        _defaultSizeDelta = _rect.sizeDelta;
    }

    public void ShowUnitsText(bool show)
    {
        if (unitsText != null)
            unitsText.gameObject.SetActive(show);
    }

    public void SetUnitsText(string text)
    {
        if (unitsText != null)
            unitsText.text = text;
    }

    // slide in from right + fade in
    public void AnimateIn(float duration = 0.3f, Action onComplete = null)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateInCoroutine(duration, onComplete));
    }

    private IEnumerator AnimateInCoroutine(float duration, Action onComplete)
    {
        float slideOffset = _defaultSizeDelta.x;
        Vector2 startPos = _defaultAnchoredPos + new Vector2(slideOffset, 0f);

        _rect.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            _rect.anchoredPosition = Vector2.Lerp(startPos, _defaultAnchoredPos, t);
            canvasGroup.alpha = t;
            yield return null;
        }

        _rect.anchoredPosition = _defaultAnchoredPos;
        canvasGroup.alpha = 1f;
        onComplete?.Invoke();
    }

    // slide out to left + fade out
    public void AnimateOut(float duration = 0.3f, Action onComplete = null)
    {
        StartCoroutine(AnimateOutCoroutine(duration, onComplete));
    }

    private IEnumerator AnimateOutCoroutine(float duration, Action onComplete)
    {
        float slideOffset = _defaultSizeDelta.x;
        Vector2 endPos = _defaultAnchoredPos - new Vector2(slideOffset, 0f);

        Vector2 startPos = _rect.anchoredPosition;
        float startAlpha = canvasGroup.alpha;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            _rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    // slide left + expand to main slot size/position
    public void Takeover(Vector2 targetPos, Vector2 targetSize, float duration = 0.4f, Action onComplete = null)
    {
        StartCoroutine(TakeoverCoroutine(targetPos, targetSize, duration, onComplete));
    }

    private IEnumerator TakeoverCoroutine(Vector2 targetPos, Vector2 targetSize, float duration, Action onComplete)
    {
        Vector2 startPos = _rect.anchoredPosition;
        Vector2 startSize = _rect.sizeDelta;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            _rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            _rect.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
            yield return null;
        }

        _rect.anchoredPosition = targetPos;
        _rect.sizeDelta = targetSize;
        _defaultAnchoredPos = targetPos;
        _defaultSizeDelta = targetSize;
        onComplete?.Invoke();
    }
}