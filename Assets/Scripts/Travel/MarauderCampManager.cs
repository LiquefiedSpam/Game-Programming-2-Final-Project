using UnityEngine;

public class MarauderCampManager : MonoBehaviour
{
    [SerializeField] private GameObject MarauderHut;
    [SerializeField] private MarauderCampData campData;

    private float percentForEncounter;
    private string travelStatus;
    private bool marauderPresent = false;

    public MarauderCampData CampData => campData; 

    public void spawnHut()
    {
        //check to see if spot is marauder or not
        if (MarauderChance())
        {
            //spawn game object for hut
        }
    }
    public bool MarauderChance()
    {
        if (percentForEncounter >= campData.GetChanceToAppear)
        {
            marauderPresent = true;
        }
        else
        {
            marauderPresent = false;
        }
        return marauderPresent;
    }

    public string EncounterDetails(bool marauderChance)
    {
        if (marauderChance)
        {
            campData.EncounteredMarauder();
            //perform ability of depleting items from inventory slots
            Inventory.Ins.RemoveRandomItems(campData.GetDangerLevel);

            travelStatus = $"You were attacked by Marauders and lost {campData.GetDangerLevel} item(s)";
        }
        else
        {
            campData.NoMarauder();
            travelStatus = $"There were no marauders here, lucky you!";
        }

        return travelStatus;
    }
}
