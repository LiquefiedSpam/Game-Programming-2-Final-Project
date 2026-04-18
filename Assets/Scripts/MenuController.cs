using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] PlayerInputController playerInputController;
    [SerializeField] private InputActionReference menuAction;
    [SerializeField] private InputActionReference mapAction;
    [SerializeField] private GameObject mapUI;

    private void OnEnable()
    {
        menuAction.action.performed += OnInventoryPressed;
        menuAction.action.Enable();

        mapAction.action.started += OnMapPressed;
        mapAction.action.canceled += OnMapReleased;
        mapAction.action.Enable();
    }

    private void OnDisable()
    {
        menuAction.action.started -= OnInventoryPressed;
        menuAction.action.Disable();

        mapAction.action.started -= OnMapPressed;
        mapAction.action.canceled -= OnMapReleased;
        mapAction.action.Disable();
    }

    private void OnMapPressed(InputAction.CallbackContext context)
    {
        mapUI.SetActive(true);
    }

    private void OnMapReleased(InputAction.CallbackContext context)
    {
        mapUI.SetActive(false);
    }

    private void OnInventoryPressed(InputAction.CallbackContext context)
    {
        if (InventoryDisplayManager.Ins.StallDisplayVisible)
        {
            Debug.Log("stall display visible");
            return;
        }
        else if (InventoryDisplayManager.Ins.InventoryDisplayVisible)
        {
            Debug.Log("Closing inventory");
            CloseInventory();
        }
        else
        {
            Debug.Log("showing inventory");
            playerInputController.OnPlayerMove += CloseInventory;
            InventoryDisplayManager.Ins.SetInventoryVisibility(true);
        }
    }

    void CloseInventory()
    {
        playerInputController.OnPlayerMove -= CloseInventory;
        InventoryDisplayManager.Ins.SetInventoryVisibility(false);
    }

    void OnDestroy()
    {
        playerInputController.OnPlayerMove -= CloseInventory;
    }
}
