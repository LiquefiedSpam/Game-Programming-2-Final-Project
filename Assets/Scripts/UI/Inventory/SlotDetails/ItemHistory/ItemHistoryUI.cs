using System.Collections.Generic;
using UnityEngine;

public class ItemHistoryUI : MonoBehaviour
{
    [SerializeField] GameObject noHistoryNotif;
    [SerializeField] Transform rowParent;
    [SerializeField] GameObject rowPrefab;

    List<ItemHistoryRow> rows = new();

    public void Show(ItemData item)
    {
        if (!History.ListingsByItem.ContainsKey(item) || History.ListingsByItem[item].Count == 0)
        {
            rowParent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            noHistoryNotif.SetActive(true);
            return;
        }

        List<PlayerListingSlot> listings = History.ListingsByItem[item];

        for (int i = rows.Count; i < listings.Count; i++)
        {
            InstantiateRow();
        }
        for (int i = 0; i < listings.Count; i++)
        {
            rows[i].SetListing(listings[i]);
            rows[i].gameObject.SetActive(true);
        }
        for (int i = listings.Count; i < rows.Count; i++)
        {
            rows[i].gameObject.SetActive(false);
        }

        noHistoryNotif.SetActive(false);
        rowParent.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        noHistoryNotif.SetActive(false);
        rowParent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void InstantiateRow()
    {
        GameObject obj = Instantiate(rowPrefab, rowParent);
        obj.SetActive(false);

        if (obj.TryGetComponent<ItemHistoryRow>(out var row))
        {
            rows.Add(row);
        }
    }
}