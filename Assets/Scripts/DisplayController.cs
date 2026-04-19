using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls whether player can open map and inventory.
/// </summary>
public class DisplayController : MonoBehaviour
{
    [SerializeField] PlayerInputController playerInputController;
    [SerializeField] private InputActionReference menuAction;
    [SerializeField] private InputActionReference mapAction;
    [SerializeField] MapDisplayManager mapDisplay;
    [SerializeField] InventoryDisplayManager inventoryDisplay;
    [SerializeField] UIManager uiManager;

    public bool DisplayInventoryEnabled { get; private set; } = true;
    public bool DisplayMapEnabled { get; private set; } = true;

    public static DisplayController Ins => instance;
    static DisplayController instance;

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
    }

    void OnDestroy()
    {
        menuAction.action.performed -= OnInventoryPressed;
        mapAction.action.started -= OnMapPressed;
        playerInputController.OnPlayerMove -= CloseInventory;
        playerInputController.OnPlayerMove -= CloseMap;
    }

    private void OnMapPressed(InputAction.CallbackContext context)
    {
        if (!CanDisplayMap())
        {
            return;
        }
        if (mapDisplay.IsVisible)
        {
            CloseMap();
        }
        else
        {
            playerInputController.OnPlayerMove += CloseMap;
            mapDisplay.Show();
        }
    }

    void CloseMap()
    {
        playerInputController.OnPlayerMove -= CloseMap;
        mapDisplay.Hide();
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
        return DisplayMapEnabled && !uiManager.DisplayBlocksOthers && !inventoryDisplay.DisplayBlocksOthers;
    }

    public bool CanDisplayInventory()
    {
        return DisplayInventoryEnabled && !uiManager.DisplayBlocksOthers && !mapDisplay.IsVisible;
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
