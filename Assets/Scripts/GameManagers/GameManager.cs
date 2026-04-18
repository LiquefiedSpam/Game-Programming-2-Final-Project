using System;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    //private Enum currentTownEnum;
    private Town currentTown;
    public static GameManager Ins => _instance;
    private static GameManager _instance;

    public Town CurrentTown => currentTown;
    bool inCutscene = false;
    public Action<bool> OnEnterExitCutscene;

    [SerializeField] private PlayerController player;
    [SerializeField] Transform TavernPoint; // this should probably be folded into a town scriptable object / data,
    //this is just here rn to continue my work


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
        currentTown = Town.TOWN_1;
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
        OnEnterExitCutscene?.Invoke(true);
        Vector3 dest = getTavernLoc();
        StartCoroutine(TavernTravelSequence(npc, dest));
    }

    private IEnumerator TavernTravelSequence(NpcBehavior npc, Vector3 dest)
    {
        Vector3 dir = (dest - npc.transform.position).normalized;
        float followDist = 2f;
        bool playerFollowing = false;
        bool npcDeparted = false;

        npc.StartCoroutine(npc.MoveToLocation(dest));

        while (Vector3.Distance(npc.transform.position, dest) > 0.01f)
        {
            float distToPlayer = Vector3.Distance(npc.transform.position, player.transform.position);

            // Wait until the NPC has first walked away before watching for the trigger
            if (!npcDeparted && distToPlayer > followDist + 1f)
                npcDeparted = true;

            if (npcDeparted && !playerFollowing && distToPlayer >= followDist)
            {
                playerFollowing = true;
                player.FollowDirection(dir);
                //UIManager.Ins.FadeToBlack();
            }

            yield return null;
        }
    }

    public void SetTown(Town town)
    {
        currentTown = town;
    }

    //should be phased out to be a town member function or something
    Vector3 getTavernLoc()
    {
        if (currentTown == Town.TOWN_1)
        {
            return TavernPoint.transform.position;
        }
        return Vector3.zero;
    }
}
