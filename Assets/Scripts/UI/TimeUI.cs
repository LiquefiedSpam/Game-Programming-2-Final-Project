using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TMP_Text unitsText;
    [SerializeField] private Image intervalImage;

    [Header("Interval Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite daytimeSprite;
    [SerializeField] private Sprite eveningSprite;
    [SerializeField] private Sprite nightSprite;

    private void Start()
    {
        DayManager.Ins.OnTimeChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (DayManager.Ins != null)
            DayManager.Ins.OnTimeChanged -= Refresh;
    }

    private void Refresh()
    {
        int units = DayManager.Ins.Units;
        int maxUnits = DayManager.Ins.UnitsPerInterval;

        timeSlider.maxValue = maxUnits;
        timeSlider.value = units;

        unitsText.text = units.ToString();

        intervalImage.sprite = DayManager.Ins.DayInterval switch
        {
            DayInterval.Morning => morningSprite,
            DayInterval.Daytime => daytimeSprite,
            DayInterval.Evening => eveningSprite,
            DayInterval.Night => nightSprite,
            _ => morningSprite
        };
    }
}