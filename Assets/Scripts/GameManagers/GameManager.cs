using System;
using UnityEngine;
using System.Collections;


//this is honestly more like 'Tavern Beer Event Manager' but like whatever that's for future me to fix
//if I keep working on this
public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    [SerializeField] Transform TavernPoint; // this should probably be folded into a town scriptable object / data,
                                            //this is just here rn to continue my work

    //private Enum currentTownEnum;
    private Town currentTown;
    public static GameManager Ins => _instance;
    private static GameManager _instance;

    public Town CurrentTown => currentTown;
    bool inCutscene = false;
    public Action<bool> OnEnterExitCutscene;

    private Vector3 tavernPlayerSpawnPt;
    private Vector3 tavernNpcSpawnPt;
    private NpcBehavior beerNpc;
    private Vector3 playerPositionBeforeCutscene;


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
        currentTown = Town.WOODED_KEEP;
        tavernPlayerSpawnPt = new Vector3(-75, 0, -75);
        tavernNpcSpawnPt = new Vector3(-74, 0, -74);
        playerPositionBeforeCutscene = Vector3.zero;
        beerNpc = null;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        UIManager.Ins.ShowDialogue(false, beerNpc.name, "What'll it be?", null);

        bool playerConfirmed = false;
        UIManager.Ins.WaitForConfirm(() => playerConfirmed = true);

        while (!playerConfirmed)
            yield return null;

        UIManager.Ins.CloseDialogue();
        UIManager.Ins.EnableTavernUI(beerNpc.name);
    }

    public void SetTown(Town town)
    {
        currentTown = town;
    }

    //should be phased out to be a town member function or something
    Vector3 getTavernLoc()
    {
        if (currentTown == Town.WOODED_KEEP)
        {
            return TavernPoint.transform.position;
        }
        return Vector3.zero;
    }

    public IEnumerator HandleBeerSelected(BeerData beer)
    {
        if (beer.price > MoneyManager.Ins.Money)
        {
            UIManager.Ins.DisplayError("not enough money!");
            yield break;
        }

        yield return StartCoroutine(UIManager.Ins.WaitForBeerButtonAnim());
        UIManager.Ins.HideTavernUI();

        //show npc-specific beer dialogue
        UIManager.Ins.ShowDialogue(false, beerNpc.name, beerNpc.largeBeerDialogue, beerNpc.portrait);

        bool confirmed = false;
        UIManager.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;

        //show generic follow-up dialogue
        UIManager.Ins.ShowDialogue(true, beerNpc.name, "By the way, since you're a traveling merchant,"
        + "I think you should know about this safe path!", beerNpc.portrait);

        confirmed = false;
        UIManager.Ins.WaitForConfirm(() => confirmed = true);
        while (!confirmed) yield return null;

        //LOGIC STUFF HERE
        int numBars = GenerateBars(beer);
        var result = PathManager.Ins.TryGetRandomInteraction(Data.CurrentTown);

        //if all paths in this town are just full of marauders for some reason, tough noogies
        if (result == null)
        {
            UIManager.Ins.ShowDialogue(true, beerNpc.name,
            "W-wait a minute! All paths around here have marauders??", beerNpc.portrait);
            confirmed = false;
            UIManager.Ins.WaitForConfirm(() => confirmed = true);
            while (!confirmed) yield return null;

            HeadBack();
        }

        //successfully found interaction to add bars to
        else
        {
            result.SetMarauderChance
        }
    }

    public IEnumerator HandleTavernQuitButton()
    {
        Debug.Log("tavern quit");
        yield return null;

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
        UIManager.Ins.ShowDialogue(false, beerNpc.name, beerNpc.returnFromTavernDialogue, beerNpc.portrait);

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
        int rapportTier = RapportManager.Ins.GetRapportLevel(beerNpc.name);
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
