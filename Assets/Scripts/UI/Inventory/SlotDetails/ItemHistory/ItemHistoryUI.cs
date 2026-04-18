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
        currentTownText.text = Util.TownToString(Data.CurrentTown);

        if (!History.ItemTownHistory[Data.CurrentTown].ContainsKey(item))
        {
            priceParent.SetActive(false);
            noHistoryNotif.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            noHistoryNotif.SetActive(false);

            ItemHistory history = History.ItemTownHistory[Data.CurrentTown][item];
            cheapPrice.Show(history.BestCheapPrice, currentTownText.text);
            targetPrice.Show(history.BestTargetPrice, currentTownText.text);
            expensivePrice.Show(history.BestExpensivePrice, currentTownText.text);

            priceParent.SetActive(true);
            gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}