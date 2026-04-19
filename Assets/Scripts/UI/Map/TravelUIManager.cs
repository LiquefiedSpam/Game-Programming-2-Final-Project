using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TravelUIManager : MonoBehaviour
{
    [Header("Choice UI")]
    [SerializeField] GameObject choiceParent;
    [SerializeField] Button upButton;
    [SerializeField] InteractionPointUI upInteractionUI;
    [SerializeField] Button downButton;
    [SerializeField] InteractionPointUI downInteractionUI;
    [Header("Interaction Point UI")]
    [SerializeField] CanvasGroup interactionCanvasGroup;
    [SerializeField] TMP_Text interactionMessageText;
    [SerializeField][TextArea(2, 5)] string maraudersMessage;
    [SerializeField][TextArea(2, 5)] string safetyMessage;
    [SerializeField] float messageHoldDuration = 3f;
    [SerializeField] float messageFadeDuration = 2f;

    public static TravelUIManager Ins => instance;
    static TravelUIManager instance;

    Coroutine interactionResultRoutine;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void ShowSafeResult()
    {
        interactionMessageText.text = safetyMessage;

        if (interactionResultRoutine != null) StopCoroutine(interactionResultRoutine);
        interactionResultRoutine = StartCoroutine(FadeInteractionResult());
    }

    public void ShowMarauderResult(int stolenItems)
    {
        if (stolenItems > 0)
        {
            string itemStr = stolenItems > 1 ? "items" : "item";
            interactionMessageText.text = maraudersMessage + $"\nYou were relieved of {stolenItems} {itemStr}.";
        }
        else
        {
            interactionMessageText.text = maraudersMessage + $"\nLuckily, you weren't carrying any items.";
        }

        if (interactionResultRoutine != null) StopCoroutine(interactionResultRoutine);
        interactionResultRoutine = StartCoroutine(FadeInteractionResult());
    }

    public async Task<ChoiceResult> AwaitChoiceResult(InteractionInfo upInteraction, InteractionInfo downInteraction)
    {
        if (choiceParent.activeSelf)
        {
            upButton.onClick.RemoveAllListeners();
            downButton.onClick.RemoveAllListeners();
        }

        TaskCompletionSource<ChoiceResult> resultTask = new();

        Data.TileDisplaySO.SetInteractionSymbols(upInteractionUI.symbolImage, upInteractionUI.levelImage, upInteraction);
        Data.TileDisplaySO.SetInteractionSymbols(downInteractionUI.symbolImage, downInteractionUI.levelImage, downInteraction);

        choiceParent.SetActive(true);
        upButton.onClick.AddListener(() => ButtonClicked(ChoiceResult.UP));
        downButton.onClick.AddListener(() => ButtonClicked(ChoiceResult.DOWN));

        while (!resultTask.Task.IsCompleted)
        {
            await Task.Yield();
        }

        return resultTask.Task.Result;

        void ButtonClicked(ChoiceResult result)
        {
            upButton.onClick.RemoveAllListeners();
            downButton.onClick.RemoveAllListeners();
            choiceParent.SetActive(false);
            resultTask.TrySetResult(result);
        }
    }

    IEnumerator FadeInteractionResult()
    {
        interactionCanvasGroup.alpha = 1f;
        interactionCanvasGroup.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageHoldDuration);

        float timePassed = 0f;
        while (interactionCanvasGroup.alpha > 0f)
        {
            yield return null;
            timePassed += Time.deltaTime;
            interactionCanvasGroup.alpha = Mathf.Max(0f, 1 - (timePassed / messageFadeDuration));
        }

        interactionCanvasGroup.gameObject.SetActive(false);
    }
}

public enum ChoiceResult
{
    UP, DOWN
}