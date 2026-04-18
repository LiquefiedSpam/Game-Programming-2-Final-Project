using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BestPriceUI : MonoBehaviour
{
    [SerializeField] EventTrigger eventTrigger;
    [SerializeField] TMP_Text priceText;
    [Header("Info")]
    [SerializeField] string reactionAsString;
    [SerializeField] GameObject infoParent;
    [SerializeField] TMP_Text neverReceivedText;
    [SerializeField] TMP_Text receivedExplanationText;

    string townString;

    void Awake()
    {
        SetEventTriggers();
    }

    public void Show(float price, string town)
    {
        if (price < 0)
        {
            priceText.text = "--";
            receivedExplanationText.gameObject.SetActive(false);
            neverReceivedText.text = NEVER_RECEIVED_TEXT.Replace("<reaction>", reactionAsString).Replace("<town>", town);
            neverReceivedText.gameObject.SetActive(true);
        }
        else
        {
            priceText.text = "$" + price.ToString("F2");
            neverReceivedText.gameObject.SetActive(false);
            receivedExplanationText.text = RECEIVED_EXPLANATION_TEXT.Replace("<price>", priceText.text).Replace("<town>", town);
            receivedExplanationText.gameObject.SetActive(true);
        }

        townString = town;
    }

    void SetEventTriggers()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry enter = new() { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((_) => SetInfoVisibility(true));
        eventTrigger.triggers.Add(enter);

        EventTrigger.Entry exit = new() { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((_) => SetInfoVisibility(false));
        eventTrigger.triggers.Add(exit);
    }

    void SetInfoVisibility(bool visible)
    {
        if (!enabled || !gameObject.activeInHierarchy) return;

        infoParent.SetActive(visible);
    }

    const string NEVER_RECEIVED_TEXT = "You've never set a price that received <reaction> in <town>.";
    const string RECEIVED_EXPLANATION_TEXT = "<price> is the highest price that received <reaction> in <town>.";
}