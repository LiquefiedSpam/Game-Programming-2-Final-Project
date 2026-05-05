using UnityEngine;
using System.Collections;

public class DayNightLighting : MonoBehaviour
{
    [Header("Sun Angles (X rotation)")]
    [SerializeField] float morningAngle = 55f;
    [SerializeField] float daytimeAngle = 90f;
    [SerializeField] float eveningAngle = 140f;
    [SerializeField] float nightAngle = 190f;

    [Header("Light Colors")]
    [SerializeField] Color morningColor = new Color(1f, 0.7f, 0.4f);
    [SerializeField] Color daytimeColor = new Color(1f, 1f, 0.9f);
    [SerializeField] Color eveningColor = new Color(1f, 0.4f, 0.2f);
    [SerializeField] Color nightColor = new Color(0.1f, 0.1f, 0.3f);

    Light _light;
    Coroutine _transitionCoroutine;

    DayInterval _lastInterval;

    void Awake()
    {
        _light = GetComponent<Light>();
    }

    void Start()
    {
        DayManager.Ins.OnTimeSet += OnIntervalChanged;
        _lastInterval = DayManager.Ins.DayInterval;
        float rot = GetAngle(_lastInterval);
        transform.rotation = Quaternion.Euler(rot, transform.eulerAngles.y, 0f);
        _light.color = GetColor(_lastInterval);

    }

    void OnDestroy()
    {
        DayManager.Ins.OnTimeSet -= OnIntervalChanged;
    }

    void OnIntervalChanged()
    {
        DayInterval interval = DayManager.Ins.DayInterval;
        if (interval == _lastInterval) return;
        _lastInterval = interval;

        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        _transitionCoroutine = StartCoroutine(Transition(GetAngle(interval), GetColor(interval)));
    }

    IEnumerator Transition(float targetAngle, Color targetColor, float duration = 2f)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(targetAngle, transform.eulerAngles.y, 0f);
        Color startColor = _light.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);
            _light.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        transform.rotation = targetRot;
        _light.color = targetColor;
    }

    float GetAngle(DayInterval interval) => interval switch
    {
        DayInterval.Morning => morningAngle,
        DayInterval.Daytime => daytimeAngle,
        DayInterval.Evening => eveningAngle,
        DayInterval.Night => nightAngle,
        _ => daytimeAngle
    };

    Color GetColor(DayInterval interval) => interval switch
    {
        DayInterval.Morning => morningColor,
        DayInterval.Daytime => daytimeColor,
        DayInterval.Evening => eveningColor,
        DayInterval.Night => nightColor,
        _ => daytimeColor
    };
}