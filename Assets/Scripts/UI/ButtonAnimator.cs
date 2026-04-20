using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] float hoverScale = 1.05f;
    [SerializeField] float popAmt = 1.2f;

    Vector3 defaultScale;
    Coroutine scaleCoroutine;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetScale(defaultScale * hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetScale(defaultScale);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetScale(defaultScale);
        RunCoroutine(UIAnimations.PopAndShrink(transform, defaultScale, popAmt));
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