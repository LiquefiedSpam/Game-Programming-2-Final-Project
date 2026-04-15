using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string title;
    public Sprite sprite;
    [Space]
    public Town town;
    public string merchant;
    public float merchantSalePrice;
    [Space]
    public float town1TargetPrice;
    public float town2TargetPrice;
    public float town3TargetPrice;
    [Space]
    public bool consumable;
    public float healthIncrease;

    // TODO actually implement all of this
    public Vector2Int GetWaitTime(float price, int amount)
    {
        return Vector2Int.one;
    }
    public CustomerReaction GetCustomerReaction(Vector2Int wait)
    {
        int rand = Random.Range(0, 5);
        return (CustomerReaction)rand;
    }
    public SaleResult GetSaleResult(CustomerReaction customerReaction)
    {
        if (customerReaction == CustomerReaction.ANGRY) return SaleResult.REJECTED;
        else return SaleResult.ACCEPTED;
    }
}
