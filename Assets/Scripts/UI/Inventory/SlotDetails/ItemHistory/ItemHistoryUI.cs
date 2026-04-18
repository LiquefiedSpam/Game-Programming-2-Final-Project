using TMPro;
using UnityEngine;

public class ItemHistoryUI : MonoBehaviour
{
    [SerializeField] GameObject noHistoryNotif;
    [SerializeField] TMP_Text currentTownText;
    [Space]
    [SerializeField] GameObject priceParent;
    [SerializeField] BestPriceUI cheapPrice;
    [SerializeField] BestPriceUI targetPrice;
    [SerializeField] BestPriceUI expensivePrice;

    public void Show(ItemData item)
    {
        currentTownText.text = Data.CurrentTown.TownToString();

        if (!History.HistoryBySaleTown[Data.CurrentTown].ContainsKey(item))
        {
            priceParent.SetActive(false);
            noHistoryNotif.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            noHistoryNotif.SetActive(false);

            ItemHistory history = History.HistoryBySaleTown[Data.CurrentTown][item];
            cheapPrice.Show(history.BestCheapPrice);
            targetPrice.Show(history.BestTargetPrice);
            expensivePrice.Show(history.BestExpensivePrice);

            priceParent.SetActive(true);
            gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}