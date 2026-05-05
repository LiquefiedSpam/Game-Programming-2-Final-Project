using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls whether player can open map and inventory.
/// </summary>
public class DisplayController : MonoBehaviour
{
    [SerializeField] PlayerInteractionManager playerInputController;
    [SerializeField] private InputActionReference menuAction;
    [SerializeField] private InputActionReference mapAction;
    [SerializeField] InventoryDisplayManager inventoryDisplay;
    [SerializeField] UIManager uiManager;

    public bool DisplayInventoryEnabled { get; private set; } = true;
    public bool DisplayMapEnabled { get; private set; } = true;

    public static DisplayController Ins => instance;
    static DisplayController instance;

    bool inCutscene = false;

    void Awake()
    {
        if (Ins != null && Ins != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        menuAction.action.performed += OnInventoryPressed;
        menuAction.action.Enable();

        mapAction.action.started += OnMapPressed;
        mapAction.action.Enable();

        TavernManager.Ins.OnEnterExitCutscene += OnEnterExitCutscene;
    }

    void OnDestroy()
    {
        menuAction.action.performed -= OnInventoryPressed;
        mapAction.action.started -= OnMapPressed;
        //        playerInputController.OnPlayerMove -= CloseInventory;
        playerInputController.OnPlayerMove -= CloseMap;
        TavernManager.Ins.OnEnterExitCutscene -= OnEnterExitCutscene;
    }

    void OnEnterExitCutscene(bool enter)
    {
        if (enter)
            inCutscene = true;

        if (enter == false)
            inCutscene = false;
    }

    private void OnMapPressed(InputAction.CallbackContext context)
    {
        if (!CanDisplayMap())
        {
            return;
        }
        if (MapDisplayManager.Ins.IsVisible)
        {
            CloseMap();
        }
        else
        {
            playerInputController.OnPlayerMove += CloseMap;
            MapDisplayManager.Ins.Show();
        }
    }

    void CloseMap()
    {
        playerInputController.OnPlayerMove -= CloseMap;
        MapDisplayManager.Ins.Hide();
    }

    private void OnInventoryPressed(InputAction.CallbackContext context)
    {
        if (!CanDisplayInventory())
        {
            Debug.Log("Apparently cannot display inventory");
            return;
        }
        else if (inventoryDisplay.InventoryDisplayVisible)
        {
            CloseInventory();
        }
        else
        {
            playerInputController.OnPlayerMove += CloseInventory;
            inventoryDisplay.SetInventoryVisibility(true);
        }
    }

    void CloseInventory()
    {
        playerInputController.OnPlayerMove -= CloseInventory;
        inventoryDisplay.SetInventoryVisibility(false);
    }

    public bool CanDisplayMap()
    {
        return DisplayMapEnabled && !uiManager.DisplayBlocksOthers && !inventoryDisplay.DisplayBlocksOthers && !inCutscene;
    }

    public bool CanDisplayInventory()
    {
        return DisplayInventoryEnabled && !uiManager.DisplayBlocksOthers && !MapDisplayManager.Ins.IsVisible && !inCutscene;
    }

    public void EnableDisplayInventory()
    {
        menuAction.action.Enable();
        DisplayInventoryEnabled = true;
    }

    public void DisableDisplayInventory()
    {
        menuAction.action.Disable();
        DisplayInventoryEnabled = true;
    }

    public void EnableDisplayMap()
    {
        mapAction.action.Enable();
        DisplayMapEnabled = true;
    }

    public void DisableDisplayMap()
    {
        mapAction.action.Disable();
        DisplayMapEnabled = false;
    }
}
