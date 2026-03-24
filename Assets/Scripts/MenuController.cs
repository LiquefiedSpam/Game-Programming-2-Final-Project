using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] private InputActionReference menuAction;
    [SerializeField] private InputActionReference mapAction;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject mapUI;

    private void OnEnable()
    {
        menuAction.action.started += OnInventoryPressed;
        menuAction.action.canceled += OnInventoryReleased;
        menuAction.action.Enable();

        mapAction.action.started += OnMapPressed;
        mapAction.action.canceled += OnMapReleased;
        mapAction.action.Enable();
    }
    private void OnDisable()
    {
        menuAction.action.started -= OnInventoryPressed;
        menuAction.action.canceled -= OnInventoryReleased;
        menuAction.action.Disable();

        mapAction.action.started -= OnMapPressed;
        mapAction.action.canceled -= OnMapReleased;
        mapAction.action.Disable();
    }

    private void OnInventoryPressed(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(true);
    }

    private void OnInventoryReleased(InputAction.CallbackContext context)
    {
        inventoryUI.SetActive(false);
    }

    private void OnMapPressed(InputAction.CallbackContext context)
    {
        mapUI.SetActive(true);
    }

    private void OnMapReleased(InputAction.CallbackContext context)
    {
        mapUI.SetActive(false);
    }
}
