using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TMP_Text unitsText;
    [SerializeField] private TimeIconBehavior timeIcon;

    [Header("Interval Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite daytimeSprite;
    [SerializeField] private Sprite eveningSprite;
    [SerializeField] private Sprite nightSprite;

    private DayInterval lastInterval;

    private void Start()
    {
        DayManager.Ins.OnTimeChanged += Refresh;
        lastInterval = DayManager.Ins.DayInterval;
        timeIcon.SetSprite(GetCorrectSprite());
    }

    private void OnDestroy()
    {
        if (DayManager.Ins != null)
            DayManager.Ins.OnTimeChanged -= Refresh;
    }

    private void Refresh()
    {
        timeSlider.maxValue = DayManager.Ins.UnitsPerInterval;
        timeSlider.value = DayManager.Ins.Units;
        unitsText.text = DayManager.Ins.Units.ToString();

        if (DayManager.Ins.DayInterval != lastInterval)
        {
            lastInterval = DayManager.Ins.DayInterval;
            timeIcon.TransitionTo(GetCorrectSprite());
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
}