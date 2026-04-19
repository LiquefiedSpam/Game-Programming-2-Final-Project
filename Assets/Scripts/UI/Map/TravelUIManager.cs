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
    [SerializeField] Button downButton;
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

    public void ShowInteractionResult(InteractionResult interactionResult)
    {
        interactionMessageText.text = interactionResult == InteractionResult.SAFE
            ? safetyMessage
            : maraudersMessage;
        interactionCanvasGroup.alpha = 1f;
        interactionCanvasGroup.gameObject.SetActive(true);

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