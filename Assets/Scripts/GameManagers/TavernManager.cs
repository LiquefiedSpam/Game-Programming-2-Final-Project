using System;
using UnityEngine;
using System.Collections;

public class TavernManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Sprite barNpcSprite;

    public static TavernManager Ins => _instance;
    private static TavernManager _instance;

    public Action<bool> OnEnterExitCutscene;

    private readonly Vector3 tavernPlayerSpawnPt = new(-75, 0, -75);
    private readonly Vector3 tavernNpcSpawnPt = new(-74, 0, -75);

    private NpcBehavior beerNpc;
    private Vector3 playerPositionBeforeCutscene;
    private int numBars = 0;
    private Action _onTavernComplete;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of TavernManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void GoToTavernAndDrink(NpcBehavior npc, Action onComplete = null)
    {
        playerPositionBeforeCutscene = player.transform.position;
        beerNpc = npc;
        _onTavernComplete = onComplete;
        OnEnterExitCutscene?.Invoke(true);
        StartCoroutine(TavernTravelSequence());
    }

    public void StartBeerInteraction(NpcBehavior npc)
    {
        beerNpc = npc;
        StartCoroutine(BeerInteractionSequence());
    }

    private IEnumerator TavernTravelSequence()
    {
        yield return StartCoroutine(TeleportRoutine());
        yield return StartCoroutine(BeerInteractionSequence());
    }

    private IEnumerator TeleportRoutine()
    {
        yield return StartCoroutine(UIManager.Ins.FadeOut());
        player.SetLocation(tavernPlayerSpawnPt);
        beerNpc.SetLocation(tavernNpcSpawnPt);
        yield return StartCoroutine(UIManager.Ins.FadeIn());
    }

    private IEnumerator BeerInteractionSequence()
    {
        DialogueUIManager.Ins.ShowDialogue("Bartender", "What'll it be?", barNpcSprite, null, showContinue: true);
        bool confirmed = false;
        DialogueDriver.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;
        DialogueUIManager.Ins.CloseDialogue();
        UIManager.Ins.EnableTavernUI(beerNpc.NpcName);
    }

    public IEnumerator HandleBeerSelected(BeerData beer)
    {
        if (beer.price > MoneyManager.Ins.Money)
        {
            UIManager.Ins.DisplayError("not enough money!");
            yield break;
        }

        MoneyManager.Ins.AddMoney(-beer.price);
        yield return StartCoroutine(UIManager.Ins.WaitForBeerButtonAnim());
        UIManager.Ins.HideBeerUI();

        numBars = GenerateBars(beer);
        string beerDialogue = GetBeerDialogue(beer.size);
        (Town t1, Town t2) = Data.CurrentTown.GetOtherTowns();

        DialogueDriver.Ins.PlaySequence(beerNpc.NpcName, beerNpc.portrait,
            new[] { beerDialogue, "By the way, since you're a traveling merchant, I got some info on safe paths from this town!" },
            () =>
            {
                DialogueUIManager.Ins.CloseDialogue();
                UIManager.Ins.EnableTavernDestUI(t1, t2);
            });
    }

    public IEnumerator HandleDestSelected(Town town)
    {
        yield return StartCoroutine(UIManager.Ins.WaitForDestButtonAnim());
        UIManager.Ins.DisableTavernUI();

        InteractionInfo pt = PathManager.Ins.GetRandomInteraction(Data.CurrentTown, town);

        MapDisplayManager.Ins.CutsceneShow(Data.CurrentTown, town);
        yield return StartCoroutine(WaitAndPlaceBars(pt));

        MapDisplayManager.Ins.AddExitButton();
        MapDisplayManager.Ins.ShowDetailsFor(Data.CurrentTown, town);

        bool mapQuit = false;
        MapDisplayManager.Ins.OnMapQuit += onQuit;
        while (!mapQuit) yield return null;
        MapDisplayManager.Ins.OnMapQuit -= onQuit;

        StartCoroutine(HeadBack());

        void onQuit() => mapQuit = true;
    }

    public IEnumerator HandleTavernQuitButton()
    {
        yield return StartCoroutine(HeadBack());
    }

    public IEnumerator HeadBack()
    {
        yield return StartCoroutine(UIManager.Ins.FadeOut());
        player.SetLocation(playerPositionBeforeCutscene);
        beerNpc.GoToDefaultLocation();
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        beerNpc.TurnAndTalk(player.transform.position);

        bool done = false;
        DialogueDriver.Ins.PlaySequence(beerNpc.NpcName, beerNpc.portrait,
            new[] { beerNpc.returnFromTavernDialogue },
            () =>
            {
                DialogueUIManager.Ins.CloseDialogue();
                OnEnterExitCutscene?.Invoke(false);
                _onTavernComplete?.Invoke();
                _onTavernComplete = null;
                done = true;
            });

        while (!done) yield return null;
    }

    private IEnumerator WaitAndPlaceBars(InteractionInfo pt)
    {
        yield return new WaitForSeconds(1f);
        pt.ModifyMarauderChance(-numBars);
    }

    private string GetBeerDialogue(BeerSize size)
    {
        return size switch
        {
            BeerSize.SMALL => beerNpc.smallBeerDialogue,
            BeerSize.MEDIUM => beerNpc.mediumBeerDialogue,
            BeerSize.LARGE => beerNpc.largeBeerDialogue,
            _ => ""
        };
    }

    int GenerateBars(BeerData beer)
    {
        int bars = 0;
        int roll = UnityEngine.Random.Range(1, 101);
        int rapportTier = RapportManager.Ins.GetRapportLevel(beerNpc.NpcName);
        int rapportModifier = 0;

        if (rapportTier == 1) rapportModifier = 3;
        if (rapportTier == 2) rapportModifier = 8;
        if (rapportTier == 3) rapportModifier = 15;
        if (rapportTier == 4) rapportModifier = 25;
        if (rapportTier == 6) rapportModifier = 100;

        roll = Mathf.Min(roll += rapportModifier, 100);

        if (roll < 51)
            bars = beer.baseBars;
        else if (roll < 86)
            bars = beer.midBars;
        else
            bars = beer.highBars;

        return bars;
    }
}
