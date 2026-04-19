using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pathing/MarauderNode", menuName = "MarauderData")]
public class MarauderCampData : ScriptableObject
{
    public float chanceToAppear = 0.5f;
    [SerializeField] private int dangerLevel = Random.Range(1, 11);
    [SerializeField] private Image dangerIcon;
    [SerializeField] private Image neutralIcon;
    [SerializeField] private Image safeIcon;
    [SerializeField] private Image fillUI;

    private Image currentIcon; //get ref to this for UI
    
    public bool marauderActuallyThere;

    public float GetChanceToAppear => chanceToAppear;
    public int GetDangerLevel => dangerLevel;

    void Awake()
    {
        DayManager.Ins.OnDayChanged += DayProgression;
    }

    void OnDestroy()
    {
        DayManager.Ins.OnDayChanged -= DayProgression;
    }

    public void EncounteredMarauder()
    {
        chanceToAppear = 1f;
        //make fillUI red and full
        currentIcon = dangerIcon;
    }

    public void NoMarauder()
    {
        chanceToAppear = 0f;
        //make fillUI green and full
        currentIcon = safeIcon;
    }

    private void DayProgression()
    {
        if (chanceToAppear > 0.5f)
        {
            chanceToAppear -= 0.1f;
            //update fillUI
        }
        else if (chanceToAppear < 0.5f)
        {
            chanceToAppear += 0.1f;
            //update fillUI
        }

        if (chanceToAppear == 0.5f)
        {
            //make the fill UI full and grey
            currentIcon = neutralIcon;
        }
    }
}
