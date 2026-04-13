using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TMP_Text unitsText;
    [SerializeField] private Image intervalImage;
    [SerializeField] private GameObject timeIconPrefab;
    [SerializeField] private GameObject timeIconPrefabHolder;

    [Header("Interval Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite daytimeSprite;
    [SerializeField] private Sprite eveningSprite;
    [SerializeField] private Sprite nightSprite;

    Sprite currentImg;
    TimeIconBehavior currentIconPrefab;
    Vector3 timeIconPrefabPos = new Vector3(362, -9, 0);



    private void Start()
    {
        DayManager.Ins.OnTimeChanged += Refresh;
        StartingIcon();
        Refresh();
    }

    private void StartingIcon()
    {
        currentImg = GetCorrectImg();
        spawnInIcon(true);
        currentIconPrefab.SetSprite(currentImg);
    }

    private void ChangeIcon()
    {
        StartCoroutine(currentIconPrefab.Fade(false));
        spawnInIcon(false);
        StartCoroutine(currentIconPrefab.Fade(true));
    }

    private void spawnInIcon(bool isStarting)
    {
        Vector3 startingRot;
        if (isStarting)
        {
            startingRot = new Vector3(0, 0, 0);
        }
        else
        {
            startingRot = new Vector3(0, 0, 50);
        }

        GameObject iconPrefab = Instantiate(timeIconPrefab, timeIconPrefabHolder.transform.position,
        Quaternion.Euler(startingRot), timeIconPrefabHolder.transform);
        currentIconPrefab = iconPrefab.GetComponent<TimeIconBehavior>();
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

        //change out icon?
        if (currentImg != GetCorrectImg())
        {
            ChangeIcon();
        }
    }

    //Gets what the displayed image SHOULD be. Does not set it on its own.
    private Sprite GetCorrectImg()
    {
        Sprite returnSprite = DayManager.Ins.DayInterval switch
        {
            DayInterval.Morning => morningSprite,
            DayInterval.Daytime => daytimeSprite,
            DayInterval.Evening => eveningSprite,
            DayInterval.Night => nightSprite,
            _ => morningSprite
        };

        return returnSprite;
    }
}