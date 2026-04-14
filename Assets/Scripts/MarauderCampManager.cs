using UnityEngine;

public class MarauderCampManager : MonoBehaviour
{
    [SerializeField] private GameObject MarauderHut;
    [SerializeField] private MarauderCampData campData;

    private float percentForEncounter;
    private string travelStatus;

    public void spawnHut()
    {
        //check to see if spot is marauder or not
        if (MarauderChance())
        {
            //spawn game object for hut
        }
    }
    private bool MarauderChance()
    {
        if (percentForEncounter >= campData.GetChanceToAppear)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string EncounterDetails()
    {
        percentForEncounter = Random.Range(0f, 1f);

        if (MarauderChance())
        {
            campData.EncounteredMarauder();
            //perform ability of depleting items from inventory slots
            PlayerInventory.Instance.LoseItems(campData.GetDangerLevel);

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
