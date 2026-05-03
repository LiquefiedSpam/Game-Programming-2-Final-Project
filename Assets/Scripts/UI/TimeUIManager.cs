using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class TimeUIManager : MonoBehaviour
{
    [Header("References")]

    //frontTimeSlider is the foreground slider-- the one that will flash visually, exposing the background slider,
    //when the player is previewing an action that will, in the end, cost time units. It will reduce its size when
    //the player takes the action, again exposing the background slider, but the background slider only 'catches up'
    //when those units are actually consumed.
    [SerializeField] private Slider frontTimeSlider;
    [SerializeField] private TMP_Text frontUnitsText;
    [SerializeField] private Slider middleTimeSlider;
    [SerializeField] private Slider backTimeSlider;
    [SerializeField] GameObject timeSliderRoot;
    [SerializeField] GameObject timeUIRoot;
    [SerializeField] private TimeIconBehavior timeIcon;
    [SerializeField] private TMP_Text unitsText;

    [Header("Interval Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite daytimeSprite;
    [SerializeField] private Sprite eveningSprite;
    [SerializeField] private Sprite nightSprite;

    [Header("Animation")]
    [SerializeField] private AnimationCurve shrinkCurve;

    private DayInterval lastInterval;

    //UnitsPerInterval * timeUnitInternalMultiplier = the internal number on the slider's slider fill. Used to make
    //smooth lerp transitions from one unit to another.
    private int timeUnitInternalMultiplier = 100;
    private Vector3 timeSliderRootDefaultScale = Vector3.one;
    float timeSliderRootExpandSize = 1.05f;

    private Coroutine previewCoroutine;
    private Coroutine timeUnitConsumeCoroutine;
    private Coroutine shadowConsumeCoroutine;
    public bool IsConsuming => timeUnitConsumeCoroutine != null || shadowConsumeCoroutine != null;

    public static TimeUIManager Ins => _instance;
    private static TimeUIManager _instance;


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of DayManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        DayManager.Ins.OnTimeUnitPreview += OnTimeUnitPreviewChanged;
        DayManager.Ins.OnTimeUnitConfirmed += StartTimeUnitConsume;
        DayManager.Ins.OnUnitsConsumed += StartShadowTimeUnitConsume;
        Refresh();
    }

    private void OnDestroy()
    {
        if (DayManager.Ins != null)
        {
            DayManager.Ins.OnTimeUnitPreview -= OnTimeUnitPreviewChanged;
            DayManager.Ins.OnTimeUnitConfirmed -= StartTimeUnitConsume;
            DayManager.Ins.OnUnitsConsumed -= StartShadowTimeUnitConsume;
        }
    }

    //refresh the UI to reflect current static state.
    private void Refresh()
    {
        //if (previewCoroutine != null || timeUnitConsumeCoroutine != null) return;

        frontTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval * timeUnitInternalMultiplier;
        frontTimeSlider.value = DayManager.Ins.Units * timeUnitInternalMultiplier;
        middleTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval * timeUnitInternalMultiplier;
        middleTimeSlider.value = DayManager.Ins.Units * timeUnitInternalMultiplier;
        backTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval * timeUnitInternalMultiplier;
        backTimeSlider.value = DayManager.Ins.Units * timeUnitInternalMultiplier;
        unitsText.text = DayManager.Ins.Units.ToString();

        if (DayManager.Ins.DayInterval != lastInterval)
        {
            lastInterval = DayManager.Ins.DayInterval;
            timeIcon.TransitionTo(GetCorrectSprite());
        }
    }

    //--enterPreview: if true, marks that the slider should enter a 'preview' state where it shows how many
    //TimeUnits will be consumed, should the player follow through on a previewed action.
    //--If false, *exit* that preview state.
    //--unitsToPreview: optionally put this in if you want to show how many time units the action will consume.
    private void OnTimeUnitPreviewChanged(bool enterPreview, int unitsToPreview = 0)
    {
        if (!enterPreview)
        {
            StopPreview();
            return;
        }

        if (previewCoroutine != null) return;

        if (timeUnitConsumeCoroutine != null)
            StartCoroutine(WaitThenPreview(unitsToPreview));
        else
            StartPreviewCoroutine(unitsToPreview);
    }

    private IEnumerator WaitThenPreview(int unitsToPreview)
    {
        yield return new WaitUntil(() => timeUnitConsumeCoroutine == null && shadowConsumeCoroutine == null);
        Refresh();
        StartPreviewCoroutine(unitsToPreview);
    }

    private Sprite GetCorrectSprite() => DayManager.Ins.DayInterval switch
    {
        DayInterval.Morning => morningSprite,
        DayInterval.Daytime => daytimeSprite,
        DayInterval.Evening => eveningSprite,
        DayInterval.Night => nightSprite,
        _ => morningSprite
    };


    //pop the size of the time unit wheel, then start the preview
    private void StartPreviewCoroutine(int unitsToPreview)
    {
        middleTimeSlider.value = DayManager.Ins.Units * timeUnitInternalMultiplier;
        middleTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval * timeUnitInternalMultiplier;

        StartCoroutine(UIAnimations.ScaleTo(timeSliderRoot.transform,
        timeSliderRootDefaultScale * timeSliderRootExpandSize, .1f));
        if (previewCoroutine != null) StopCoroutine(previewCoroutine);
        previewCoroutine = StartCoroutine(PreviewCoroutine(unitsToPreview));
    }

    IEnumerator PreviewCoroutine(int unitsToPreview)
    {
        float reductionDuration = 0.5f;
        while (true)
        {
            float elapsed = 0f;
            int maxInt = DayManager.Ins.Units * timeUnitInternalMultiplier;
            int intToElapse = Mathf.Max((DayManager.Ins.Units - unitsToPreview) * timeUnitInternalMultiplier, 0);
            frontTimeSlider.value = maxInt;
            //add logic for if this goes past the current interval's units!
            while (elapsed < reductionDuration)
            {
                //Reduce over time smoothly based on frame rate
                elapsed += Time.deltaTime;
                float current = Mathf.Lerp(maxInt, intToElapse, elapsed / reductionDuration);
                frontTimeSlider.value = current;
                //Wait until next frame
                yield return null;
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    public void StopPreview()
    {
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
            frontTimeSlider.value = DayManager.Ins.Units * timeUnitInternalMultiplier;
            previewCoroutine = null;
        }
        StartCoroutine(UIAnimations.ScaleTo(timeSliderRoot.transform, timeSliderRootDefaultScale, .3f));
        Refresh();
    }

    private void StartShadowTimeUnitConsume(int units)
    {
        float capturedValue = middleTimeSlider.value;
        shadowConsumeCoroutine = StartCoroutine(WaitThenShadow(units, capturedValue));
    }

    private IEnumerator WaitThenShadow(int units, float fromValue)
    {
        yield return new WaitUntil(() => timeUnitConsumeCoroutine == null);
        shadowConsumeCoroutine = StartCoroutine(TimeUnitConsume(units, middleTimeSlider, fromValue, isShadow: true));
    }

    private void StartTimeUnitConsume(int units)
    {
        Debug.Log($"StartTimeUnitConsume called, units={units}\n{System.Environment.StackTrace}");
        if (previewCoroutine != null) { StopCoroutine(previewCoroutine); previewCoroutine = null; }
        float maxValue = DayManager.Ins.Units * timeUnitInternalMultiplier;
        frontTimeSlider.value = maxValue;
        timeUnitConsumeCoroutine = StartCoroutine(TimeUnitConsume(units, frontTimeSlider, maxValue, isShadow: false));
    }

    private IEnumerator TimeUnitConsume(int units, Slider slider, float maxValue, bool isShadow)
    {
        float reductionDuration = 0.7f;
        float elapsed = 0f;
        float amtToElapse = Mathf.Max(maxValue - (units * timeUnitInternalMultiplier), 0);
        slider.value = maxValue;

        while (elapsed < reductionDuration)
        {
            elapsed += Time.deltaTime;
            float t = shrinkCurve.Evaluate(elapsed / reductionDuration);
            slider.value = Mathf.Lerp(maxValue, amtToElapse, t);
            yield return null;
        }

        slider.value = amtToElapse;

        if (isShadow) { shadowConsumeCoroutine = null; Refresh(); }
        else timeUnitConsumeCoroutine = null;
    }


    public void SetTimeUIVisibility(bool visible)
    {
        timeUIRoot.gameObject.SetActive(visible);
    }

    private int TUTointernalInt(int timeUnits)
    {
        return timeUnits * timeUnitInternalMultiplier;
    }
}