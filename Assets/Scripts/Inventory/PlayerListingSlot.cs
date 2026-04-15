using UnityEngine;

public class PlayerListingSlot : Slot
{
    float hoursInDay = 3;
    public const float MIN_PRICE = 0.5f;

    public float ListedPrice { get; private set; }
    public bool HasResult { get; private set; }
    public SaleResult SaleResult { get; private set; }
    public CustomerReaction CustomerReaction { get; private set; }
    public Sprite ReactionSprite { get; private set; }
    public Vector2Int ResultWaitTime { get; private set; }
    public Town SoldInTown { get; private set; }
    public string ID { get; private set; }
    Vector2Int timePassed;


    public PlayerListingSlot(ItemData itemData, int itemAmount, float price, Town inTown) : base(itemData, itemAmount)
    {
        item = itemData;
        amount = itemAmount;
        ListedPrice = price;
        SoldInTown = inTown;
        ID = System.Guid.NewGuid().ToString();
        BeginWaitForResult();
    }

    public PlayerListingSlot(Slot slot, float price) : base(slot.item, slot.amount)
    {
        item = slot.item;
        amount = slot.amount;
        ListedPrice = price;
    }

    public void SetPrice(float price)
    {
        ListedPrice = price;
        OnSlotChanged?.Invoke();
        BeginWaitForResult();
    }

    public void CancelSale()
    {
        // unsubscribe from wait time methods
        // TODO actually do this
        // InputHandler.OnHourPassed -= OnHourIncremented;
    }

    void BeginWaitForResult()
    {
        timePassed = Vector2Int.zero;
        HasResult = false;
        // get wait time from item and set ResultWaitTime
        // TODO actually do this
        ResultWaitTime = item.GetWaitTime(ListedPrice, amount);

        if (SaleResult != SaleResult.WAITING) // i.e. if not already subscribed
        {
            // TODO actually do this
            // subscribe OnHourIncremented to time passing actions
            // InputHandler.OnHourPassed += OnHourIncremented;
        }
    }

    void EndWaitForResult()
    {
        // get sale result from item and set SaleResult
        // get CustomerReaction from item and set CustomerReaction
        // TODO actually do this
        CustomerReaction = item.GetCustomerReaction(ResultWaitTime);
        CustomerReactions reactions = Resources.Load<CustomerReactions>("CustomerReactions");
        ReactionSprite = reactions.GetReactionSprite(CustomerReaction);

        SaleResult = item.GetSaleResult(CustomerReaction);
        HasResult = true;

        History.AddListing(this);
    }

    void OnHourIncremented()
    {
        timePassed.y += 1;
        if (timePassed.y > hoursInDay)
        {
            timePassed.x++;
            timePassed.y = 0;
        }
        if (timePassed.x > ResultWaitTime.x
            || (timePassed.x == ResultWaitTime.x && timePassed.y >= ResultWaitTime.y))
        {
            EndWaitForResult();
        }
    }
}

public enum SaleResult
{
    NONE, WAITING, ACCEPTED, REJECTED
}