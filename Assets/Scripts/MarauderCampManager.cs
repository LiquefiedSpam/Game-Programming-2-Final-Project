using UnityEngine;

public class MarauderCampManager : MonoBehaviour
{
    [SerializeField] private GameObject MarauderHut;
    public void spawnHut()
    {
        //check to see if spot is marauder or not
        if (MarauderChance())
        {

        }
    }
    public bool MarauderChance()
    {
        return false;
    }
}
