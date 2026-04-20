using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float hoverScale = 1.05f;
    [SerializeField] float popAmt = 1.2f;

    [Header("Hover Text (optional)")]
    [SerializeField] TextMeshProUGUI hoverText;
    public string hoverString = "$15";

    Vector3 defaultScale;
    Coroutine scaleCoroutine;

    public event Action OnClickAnimFinished;

    void Awake()
    {
        defaultScale = transform.localScale;
        if (hoverText != null)
            hoverText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetScale(defaultScale * hoverScale);
        if (hoverText != null)
        {
            hoverText.text = hoverString;
            hoverText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetScale(defaultScale);
        if (hoverText != null)
            hoverText.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetScale(defaultScale);
        StartCoroutine(RunClickAnim());
    }

    IEnumerator RunClickAnim()
    {
        yield return StartCoroutine(UIAnimations.PopAndShrink(transform, defaultScale, popAmt));
        OnClickAnimFinished?.Invoke();
    }

    void SetScale(Vector3 target)
    {
        RunCoroutine(UIAnimations.ScaleTo(transform, target, 0.1f));
    }

    void RunCoroutine(IEnumerator routine)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(routine);
    }
}