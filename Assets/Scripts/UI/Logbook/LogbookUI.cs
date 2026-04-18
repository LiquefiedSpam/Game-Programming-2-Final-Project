using UnityEngine;

public class LogbookUI : MonoBehaviour
{
    [Header("Item List")]
    [SerializeField] GameObject listParent;
    [SerializeField] Transform town1SlotParent;
    [SerializeField] Transform town2SlotParent;
    [SerializeField] Transform town3SlotParent;
    [SerializeField] GameObject logSlotPrefab;
    [Header("Item Details")]
    [SerializeField] GameObject detailsParent;

    public void Show()
    {

    }

    void SetItemList()
    {
        foreach (var i in History.ObtainedItems)
        {
            AddToTownList(i.town);
        }
    }

    void AddToTownList(Town town)
    {

    }
}