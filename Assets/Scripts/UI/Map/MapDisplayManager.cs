using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplayManager : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] Button exitButton;
    [Header("Entire Map")]
    [SerializeField] GameObject entireMapParent;
    [SerializeField] Button woodSandButton;
    [SerializeField] Button woodStoneButton;
    [SerializeField] Button stoneSandButton;
    [Header("Path Details")]
    [SerializeField] GameObject pathDetailsParent;
    [SerializeField] Button backButton;
    [SerializeField] TMP_Text townAText;
    [SerializeField] TMP_Text townBText;
    [SerializeField] TileUI[] townATileUI;
    [SerializeField] TileUI[] townBTileUI;
    [Header("References")]
    [SerializeField] PathManager pathManager;

    public bool IsVisible => root.activeInHierarchy;
    public static MapDisplayManager Ins => _instance;
    private static MapDisplayManager _instance;
    public Action OnMapQuit;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError($"Multiple instances of MapDisplayManager in scene, destroying component on {gameObject.name}");
            Destroy(this);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        UIManager.Ins.OnDisplayBlocksOthers += Hide;
    }

    void OnDestroy()
    {
        if (UIManager.Ins != null) UIManager.Ins.OnDisplayBlocksOthers -= Hide;
    }

    public void Show()
    {
        exitButton.onClick.AddListener(Hide);
        woodSandButton.onClick.AddListener(() => ShowDetailsFor(Town.WOODED_KEEP, Town.SANDY_STALLS));
        woodStoneButton.onClick.AddListener(() => ShowDetailsFor(Town.WOODED_KEEP, Town.STONE_SANCTUARY));
        stoneSandButton.onClick.AddListener(() => ShowDetailsFor(Town.STONE_SANCTUARY, Town.SANDY_STALLS));
        backButton.onClick.AddListener(ShowEntireMap);

        pathDetailsParent.SetActive(false);
        entireMapParent.SetActive(true);
        root.SetActive(true);
    }

    public void Hide()
    {
        exitButton.onClick.RemoveAllListeners();
        woodSandButton.onClick.RemoveAllListeners();
        woodStoneButton.onClick.RemoveAllListeners();
        stoneSandButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        entireMapParent.SetActive(false);
        pathDetailsParent.SetActive(false);
        root.SetActive(false);
        OnMapQuit?.Invoke();
    }

    //specific instructions for showing based on the tavern cutscene
    public void CutsceneShow(Town townA, Town townB)
    {
        root.SetActive(true);
        ShowDetailsFor(townA, townB);
    }

    //also used in cutscene stuff
    public void AddExitButton()
    {
        exitButton.onClick.AddListener(Hide);
    }

    public void ShowDetailsFor(Town townA, Town townB)
    {
        entireMapParent.SetActive(false);

        PathTileInfo info = pathManager.Tiles[townA][townB];

        for (int i = 0; i < PathTileInfo.TILES_PER_TOWN; i++)
        {
            townATileUI[i].ShowTile(info.townATiles[i]);
            townBTileUI[i].ShowTile(info.townBTiles[i]);
        }
        townAText.text = townA.TownToString();
        townBText.text = townB.TownToString();

        pathDetailsParent.SetActive(true);
    }

    void ShowEntireMap()
    {
        entireMapParent.SetActive(true);
        pathDetailsParent.SetActive(false);
    }
}