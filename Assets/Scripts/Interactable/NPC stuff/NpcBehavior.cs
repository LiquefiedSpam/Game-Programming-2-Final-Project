using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Animator))]

//NPC behavior script for NPCs in the towns.
public class NpcBehavior : InteractableBehavior
{
    [Header("Assign if merchant")]
    [SerializeField] MerchantStall merchantStall;
    [SerializeField] AudioSource audioSource;


    [Header("Identity")]
    [SerializeField] string name;
    [SerializeField] Sprite portrait;

    [Header("Dialogue")]
    [TextArea(2, 10)][SerializeField] string morningDialogue;
    [TextArea(2, 10)][SerializeField] string dayDialogue;
    [TextArea(2, 10)][SerializeField] string eveningDialogue;
    [TextArea(2, 10)][SerializeField] string nightDialogue;
    [SerializeField] DialogueTemplate template; //data template for what dialogue options this NPC has.
    [SerializeField] List<DialogueTextOverride> overrides; //override the default flavor text response for options.
    List<DialogueOptionInstance> runtimeOptions; //the specific set of option instances this NPC has.

    public override InteractableType Type => InteractableType.NPC;
    public static NpcBehavior InteractingWith;

    public bool CanInteract = true;
    float speed = 2f;
    Vector3 defaultPos;
    Vector3 defaultRot;



    Animator animator;
    Quaternion defaultRotation;
    Coroutine _rotateCoroutine;

    protected override void Awake()
    {
        base.Awake();
        DefaultLocation();
        SyncOverrides();
        BuildRuntimeOptions();
        animator = GetComponent<Animator>();
        defaultRotation = transform.rotation;

        if (interactableIcon != null)
            bubbleScript = interactableIcon.GetComponent<BubbleScript>();
    }

    void Start()
    {
        GameManager.Ins.OnEnterExitCutscene += EnterExitCutscene;
    }

    void OnDisable()
    {
        GameManager.Ins.OnEnterExitCutscene -= EnterExitCutscene;
    }

    public void DefaultLocation()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation.eulerAngles;
    }

    public override void Interact(Vector3 playerPos)
    {

        //setup
        if (!CanInteract)
            return;

        if (InteractingWith != null)
        {
            Debug.LogError($"Already interacting with {InteractingWith.gameObject.name}");
            return;
        }
        base.Interact(playerPos);
        InteractingWith = this;

        if (bubbleScript != null)
            bubbleScript.SetExhausted();

        //turn to face player
        SetAnimation("isTurning");
        Vector3 direction = playerPos - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        StartRotate(targetRotation, "isTalking");

        //now, dialogue
        UIManager.Ins.ShowDialogue(false, name, fetchDialogue(), portrait, GetAbleOptions());
        DayManager.Ins.ConsumeUnit(1);
        RapportManager.Ins.AddRapport(name, 1);
    }

    public void HandleOptionSelected(DialogueOptionInstance option)
    {
        if (!EvaluateCondition(option.definition.label))
            return;

        if (option.definition.label != DialogueLabel.Leave)
        {
            // Hi Morgan I am passing purchase dialogue to InventoryDisplayManager to show in merchant stall UI
            // in HandleInternalLogic, hence this if-else statement
            // - Marcella
            if (option.definition.label == DialogueLabel.Purchase)
            {
                InventoryDisplayManager.Ins.ShowMerchantStall(merchantStall, name, option.response, portrait);
            }
            else
            {
                UIManager.Ins.ShowDialogue(true, name, option.response, portrait);
            }
        }
        else
        {
            UIManager.Ins.ShowDialogue(false, name, option.response, portrait);
        }

        //outdated
        //option.action?.Execute(this);

        HandleInternalLogic(option.definition.label);
    }

    private void HandleInternalLogic(DialogueLabel label)
    {
        switch (label)
        {
            case DialogueLabel.Drink:
                GameManager.Ins.GoToTavernAction(this);
                break;

            case DialogueLabel.Purchase:
                audioSource.Play();
                UIManager.Ins.CloseDialogue(); // dialogue shown in merchant stall display
                break;

            case DialogueLabel.Leave:
                Debug.Log("Leave logic");
                break;
        }
    }

    string fetchDialogue()
    {
        if (DayManager.Ins.DayInterval == DayInterval.Morning)
            return morningDialogue;

        if (DayManager.Ins.DayInterval == DayInterval.Daytime)
            return dayDialogue;

        if (DayManager.Ins.DayInterval == DayInterval.Evening)
            return eveningDialogue;

        if (DayManager.Ins.DayInterval == DayInterval.Night)
            return nightDialogue;

        return dayDialogue;
    }

    public override void Quit()
    {
        UIManager.Ins.CloseDialogue();
        if (merchantStall != null && audioSource != null)
        {
            audioSource.Stop();
            InventoryDisplayManager.Ins.HideMerchantStall();
        }
        InteractingWith = null;

        if (inCutscene) return;

        StartRotate(defaultRotation, "isIdle");
    }

    public override void TriggerIconPopAndShrink()
    {
        if (interactableIcon != null)
            bubbleScript.StartCoroutine(bubbleScript.PopAndShrink());

        if (!bubbleScript.IsExhausted())
        {
            bubbleScript.StartCoroutine(bubbleScript.SpawnHeart());
        }
    }


    /*
    *DIALOGUE OPTIONS SETUP + FLAVOR RESPONSE HANDLING
    */

    //ensures every option in the template has a matching override
    void SyncOverrides()
    {
        if (overrides == null)
            overrides = new List<DialogueTextOverride>();

        // Add missing entries from template
        foreach (var def in template.options)
        {
            if (!HasOverride(def))
            {
                overrides.Add(new DialogueTextOverride
                {
                    definition = def
                });
            }
        }

        //remove invalid ones
        overrides.RemoveAll(o => !template.options.Contains(o.definition));
    }

    //take the overrides in the inspector, if any, and bake them into options.
    void BuildRuntimeOptions()
    {
        runtimeOptions = new List<DialogueOptionInstance>();

        foreach (var def in template.options)
        {
            var ov = overrides.Find(o => o.definition == def);

            runtimeOptions.Add(new DialogueOptionInstance
            {
                definition = def,
                response = ov != null ? ov.responseOverride : GetDefaultOptionResponseText(def),
            });
        }
    }

    bool HasOverride(DialogueOptionDefinition def)
    {
        return overrides.Exists(o => o.definition == def);
    }

    //default flavor text response from an option selection
    string GetDefaultOptionResponseText(DialogueOptionDefinition def)
    {
        switch (def.label)
        {
            case DialogueLabel.Drink: return "A drink? I'd love to!";
            case DialogueLabel.Purchase: return "Take a look at my wares.";
            case DialogueLabel.Leave: return "Goodbye.";
        }

        return "";
    }

    List<DialogueOptionInstance> GetAbleOptions()
    {
        List<DialogueOptionInstance> currentOptions = new List<DialogueOptionInstance>();
        foreach (DialogueOptionInstance doi in runtimeOptions)
        {
            if (EvaluateCondition(doi.definition.label))
                currentOptions.Add(doi);
        }

        return currentOptions;
    }

    public bool EvaluateCondition(DialogueLabel label)
    {
        switch (label)
        {
            case DialogueLabel.Drink:
                return DayManager.Ins.DayInterval == DayInterval.Evening;

            case DialogueLabel.Purchase:
                if (DayManager.Ins.DayInterval == DayInterval.Night
                || DayManager.Ins.DayInterval == DayInterval.Evening)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            case DialogueLabel.Leave:
                //only have leave as a valid option if at least one other option accompanies it
                return runtimeOptions.Exists(o =>
        o.definition.label != DialogueLabel.Leave &&
        EvaluateCondition(o.definition.label));
        }
        return true;
    }

    /*
    *ANIMATION HANDLING
    */

    //set the provided string's animation bool to true; turn off all others. isTurning is the same animation
    //for walking.
    void SetAnimation(string anim)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isTalking", false);
        animator.SetBool("isTurning", false);
        animator.SetBool(anim, true);
    }

    private void StartRotate(Quaternion targetRot, string targetAnim)
    {
        if (_rotateCoroutine != null) StopCoroutine(_rotateCoroutine);
        _rotateCoroutine = StartCoroutine(RotateCoroutine(targetRot, targetAnim));
    }

    private IEnumerator RotateCoroutine(Quaternion targetRot, string targetAnim)
    {
        SetAnimation("isTurning");
        float speed = 180f;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                speed * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRot;
        SetAnimation(targetAnim);
    }


    //triggered movement stuff
    public IEnumerator MoveToLocation(Vector3 dest)
    {
        dest.y = transform.position.y;

        Vector3 dir = (dest - transform.position).normalized;
        Quaternion targetRot = dir != Vector3.zero ? Quaternion.LookRotation(dir) : transform.rotation;
        yield return StartCoroutine(RotateCoroutine(targetRot, "isTurning"));

        while (Vector3.Distance(transform.position, dest) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = dest;
        SetAnimation("isIdle");
    }

    void EnterExitCutscene(bool enter)
    {
        interactableIcon.SetActive(!enter);
        CanInteract = !enter;
        inCutscene = enter;

        if (!enter)
        {
            DefaultLocation();
        }
    }
}