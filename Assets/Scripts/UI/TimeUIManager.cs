using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    [SerializeField] private Image middleTimeSliderFill;
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

    private DayInterval lastInterval;

    //UnitsPerInterval * timeUnitInternalMultiplier = the internal number on the slider's slider fill. Used to make
    //smooth lerp transitions from one unit to another.
    private int timeUnitInternalMultiplier = 1;
    //public int TimeUnitInternalMultiplier => timeUnitInternalMultiplier;

    private Color backTimeSliderDefaultColor = Color.red;
    private Color frontTimeSliderDefaultColor;
    private Vector3 timeSliderRootDefaultScale;
    float timeSliderRootExpandSize;

    private Coroutine previewCoroutine;


    private void Start()
    {
        DayManager.Ins.OnTimeChanged += Refresh;
        DayManager.Ins.OnTimeUnitPreview += Refresh;
        lastInterval = DayManager.Ins.DayInterval;
        timeIcon.SetSprite(GetCorrectSprite());
        timeSliderRootDefaultScale = timeSliderRoot.transform.localScale;
        timeSliderRootExpandSize = 1.05f;
    }

    private void OnDestroy()
    {
        if (DayManager.Ins != null)
            DayManager.Ins.OnTimeChanged -= Refresh;
    }


    //refresh the UI to reflect current static state.
    private void Refresh()
    {
        frontTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval;
        frontTimeSlider.value = DayManager.Ins.Units;
        unitsText.text = DayManager.Ins.Units.ToString();

        backTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval;
        backTimeSlider.value = DayManager.Ins.Units;

        middleTimeSlider.maxValue = DayManager.Ins.UnitsPerInterval;
        middleTimeSlider.value = DayManager.Ins.Units;

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
    private void Refresh(bool enterPreview, int unitsToPreview = 0)
    {
        if (!enterPreview)
        {
            StopPreview();
            Refresh();
            return;
        }
        else
        {
            //return if this call is redundant
            if (previewCoroutine != null)
                return;

            frontTimeSlider.value =
            TUTointernalInt(DayManager.Ins.Units - unitsToPreview); //will handle cases for when it's below 0 later

            StartPreviewCoroutine();
        }
    }

    private Sprite GetCorrectSprite() => DayManager.Ins.DayInterval switch
    {
        DayInterval.Morning => morningSprite,
        DayInterval.Daytime => daytimeSprite,
        DayInterval.Evening => eveningSprite,
        DayInterval.Night => nightSprite,
        _ => morningSprite
    };

    private void StartPreviewCoroutine()
    {
        Debug.Log("got here");
        StartCoroutine(UIAnimations.ScaleTo(timeSliderRoot.transform,
        timeSliderRootDefaultScale * timeSliderRootExpandSize, .1f));
        if (previewCoroutine != null) StopCoroutine(previewCoroutine);
        previewCoroutine = StartCoroutine(PreviewCoroutine());
    }

    IEnumerator PreviewCoroutine()
    {
        yield return UIAnimations.LerpAlpha(middleTimeSliderFill, 1, .8f, .2f);
        while (true)
        {
            // Fade out
            yield return UIAnimations.LerpAlpha(middleTimeSliderFill, .8f, .1f, .8f);
            // Fade in
            yield return UIAnimations.LerpAlpha(middleTimeSliderFill, .1f, .8f, .8f);

            yield return new WaitForSeconds(.3f);
        }
    }

    public void StopPreview()
    {
        if (previewCoroutine != null)
        {
            StopCoroutine(previewCoroutine);
            previewCoroutine = null;
        }

        StartCoroutine(UIAnimations.ScaleTo(timeSliderRoot.transform, timeSliderRootDefaultScale, .3f));
        middleTimeSliderFill.SetAlpha(1);
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