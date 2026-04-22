using System;
using UnityEngine;
using System.Collections;

//this is honestly more like 'Tavern Beer Event Manager' but like whatever that's for future me to fix
//if I keep working on this
public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] private Sprite barNpcSprite;


    public static GameManager Ins => _instance;
    private static GameManager _instance;
    bool inCutscene = false;
    public Action<bool> OnEnterExitCutscene;

    private Vector3 tavernPlayerSpawnPt;
    private Vector3 tavernNpcSpawnPt;
    private NpcBehavior beerNpc;
    private Vector3 playerPositionBeforeCutscene;

    private int numBars = 0;


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of GameManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }

        tavernPlayerSpawnPt = new Vector3(-75, 0, -75);
        tavernNpcSpawnPt = new Vector3(-74, 0, -75);
        playerPositionBeforeCutscene = Vector3.zero;
        beerNpc = null;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //MapDisplayManager.Ins.OnMapQuit += ResumeAfterMap;
    }

    void OnDisable()
    {
        // MapDisplayManager.Ins.OnMapQuit -= ResumeAfterMap;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToTavernAction(NpcBehavior npc)
    {
        playerPositionBeforeCutscene = player.transform.position;
        beerNpc = npc;
        OnEnterExitCutscene?.Invoke(true);
        Vector3 dest = getTavernLoc();
        StartCoroutine(TavernTravelSequence(dest));
    }

    private IEnumerator TavernTravelSequence(Vector3 dest)
    {
        Vector3 dir = (dest - beerNpc.transform.position).normalized;
        float followDist = 2f;
        bool playerFollowing = false;
        bool npcDeparted = false;

        beerNpc.StartCoroutine(beerNpc.MoveToLocation(dest));

        while (Vector3.Distance(beerNpc.transform.position, dest) > 0.01f)
        {
            float distToPlayer = Vector3.Distance(beerNpc.transform.position, player.transform.position);

            // Wait until the NPC has first walked away before watching for the trigger
            if (!npcDeparted && distToPlayer > followDist + 1f)
                npcDeparted = true;

            if (npcDeparted && !playerFollowing && distToPlayer >= followDist)
            {
                UIManager.Ins.CloseDialogue();
                playerFollowing = true;
                player.FollowDirection(dir);
                yield return TeleportRoutine();
                yield return StartCoroutine(TavernInteraction());
            }

            yield return null;
        }
    }

    private IEnumerator TeleportRoutine()
    {
        yield return StartCoroutine(UIManager.Ins.FadeOut());
        player.StopMoveInDirection();
        player.SetLocation(tavernPlayerSpawnPt);
        beerNpc.SetLocation(tavernNpcSpawnPt);
        yield return StartCoroutine(UIManager.Ins.FadeIn());
    }

    private IEnumerator TavernInteraction()
    {
        UIManager.Ins.ShowDialogue(false, "Bartender", "What'll it be?", barNpcSprite);

        bool playerConfirmed = false;
        UIManager.Ins.WaitForConfirm(() => playerConfirmed = true);

        while (!playerConfirmed)
            yield return null;

        UIManager.Ins.CloseDialogue();
        UIManager.Ins.EnableTavernUI(beerNpc.NpcName);
    }

    //should be phased out to be a town member function or something
    Vector3 getTavernLoc()
    {
        return Data.ClosestInn.transform.position;
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

        string beerDialogue = "";
        if (beer.size == BeerSize.SMALL)
            beerDialogue = beerNpc.smallBeerDialogue;

        if (beer.size == BeerSize.MEDIUM)
            beerDialogue = beerNpc.mediumBeerDialogue;

        if (beer.size == BeerSize.LARGE)
            beerDialogue = beerNpc.largeBeerDialogue;

        //show npc-specific beer dialogue
        UIManager.Ins.ShowDialogue(true, beerNpc.NpcName, beerDialogue, beerNpc.portrait);

        bool confirmed = false;
        UIManager.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;

        //show generic follow-up dialogue
        UIManager.Ins.ShowDialogue(true, beerNpc.NpcName, "By the way, since you're a traveling merchant,"
        + "I got some info on safe paths from this town!", beerNpc.portrait);

        numBars = GenerateBars(beer);

        confirmed = false;
        UIManager.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;

        UIManager.Ins.CloseDialogue();
        (Town, Town) towns = Data.CurrentTown.GetOtherTowns();
        UIManager.Ins.EnableTavernDestUI(towns.Item1, towns.Item2);
    }

    public IEnumerator HandleDestSelected(Town town)
    {
        Debug.Log(numBars);
        yield return StartCoroutine(UIManager.Ins.WaitForDestButtonAnim());
        UIManager.Ins.DisableTavernUI();

        InteractionInfo pt = PathManager.Ins.GetRandomInteraction(Data.CurrentTown, town);

        MapDisplayManager.Ins.CutsceneShow(Data.CurrentTown, town);
        yield return StartCoroutine(WaitAndPlaceBars(pt));

        MapDisplayManager.Ins.AddExitButton();
        MapDisplayManager.Ins.ShowDetailsFor(Data.CurrentTown, town);

        // wait for the player to quit the map
        bool mapQuit = false;
        MapDisplayManager.Ins.OnMapQuit += onQuit;
        while (!mapQuit) yield return null;
        MapDisplayManager.Ins.OnMapQuit -= onQuit;

        StartCoroutine(HeadBack());

        void onQuit() => mapQuit = true;
    }

    IEnumerator WaitAndPlaceBars(InteractionInfo pt)
    {
        Debug.Log(pt.MarauderChance);
        yield return new WaitForSeconds(1f);
        pt.ModifyMarauderChance(-numBars);
        Debug.Log(pt.MarauderChance);
    }

    public IEnumerator HandleTavernQuitButton()
    {
        Debug.Log("tavern quit");
        yield return StartCoroutine(HeadBack());
    }

    public void ResumeAfterMap()
    {
        if (!inCutscene)
            return;

        Debug.Log("heading back");
        StartCoroutine(HeadBack());
    }

    public IEnumerator HeadBack()
    {
        bool confirmed = false;

        //heading back
        yield return StartCoroutine(UIManager.Ins.FadeOut());
        player.SetLocation(playerPositionBeforeCutscene);
        beerNpc.GoToDefaultLocation();
        yield return StartCoroutine(UIManager.Ins.FadeIn());

        // closing dialogue
        beerNpc.TurnAndTalk(player.transform.position);
        UIManager.Ins.ShowDialogue(false, beerNpc.NpcName, beerNpc.returnFromTavernDialogue, beerNpc.portrait);

        confirmed = false;
        UIManager.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;

        beerNpc.FlushTally();
        UIManager.Ins.CloseDialogue();
        OnEnterExitCutscene?.Invoke(false);
    }

    //generate number of bars the NPC adds to a space
    int GenerateBars(BeerData beer)
    {
        int bars = 0;
        int roll = UnityEngine.Random.Range(1, 101);
        int rapportTier = RapportManager.Ins.GetRapportLevel(beerNpc.NpcName);
        int rapportModifier = 0;

        if (rapportTier == 1)
            rapportModifier = 3;

        if (rapportTier == 2)
            rapportModifier = 8;

        if (rapportTier == 1)
            rapportModifier = 15;

        if (rapportTier == 1)
            rapportModifier = 25;

        if (rapportTier == 6)
            rapportModifier = 100;


        roll = Mathf.Min(roll += rapportModifier, 100);


        if (roll < 51)
        {
            bars = beer.baseBars;
        }
        else if (roll < 86)
        {
            bars = beer.midBars;
        }
        else
        {
            bars = beer.highBars;
        }

        return bars;
    }
}
