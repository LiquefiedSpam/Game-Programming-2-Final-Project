using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHistoryRow : MonoBehaviour
{
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text boughtForText;
    [SerializeField] TMP_Text soldForText;
    [SerializeField] TMP_Text profitText;
    [SerializeField] Image reactionImage;
    [SerializeField] TMP_Text reactionText;
    [SerializeField] TMP_Text townText;
    [Space]
    [SerializeField] Color negativeColor;
    [SerializeField] Color positiveColor;

    public void SetListing(PlayerListingSlot listing)
    {
        amountText.text = "x" + listing.amount.ToString();
        boughtForText.text = "- $" + listing.GetMerchantPrice().ToString("F2");
        reactionImage.sprite = listing.ReactionSprite;
        townText.text = listing.SoldInTown.ToString();
        soldForText.text = "+ $" + listing.ListedPrice.ToString("F2");

        if (listing.SaleResult != SaleResult.REJECTED)
        {
            float profit = Mathf.RoundToInt((listing.ListedPrice / listing.GetMerchantPrice() - 1) * 100f);
            string sign = profit >= 0 ? "+ " : "- ";
            profitText.color = profit >= 0 ? positiveColor : negativeColor;
            profitText.text = sign + Mathf.Abs(profit).ToString() + "%";

            reactionText.text = $"{listing.ResultWaitTime.x} days,\n{listing.ResultWaitTime.y} hours";
        }
        else
        {
            soldForText.text = "--";
            profitText.color = negativeColor;
            profitText.text = "--";
            reactionText.text = "REJECTED";
        }
    }
}