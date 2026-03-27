using UnityEngine;
using UnityEngine.InputSystem;
public class RTSInputManager : MonoBehaviour
{
    public static RTSInputManager Instance { get; private set; }

    [SerializeField] private InputActionAsset inputActions;

    private InputAction panAction;
    private InputAction zoomAction;
    private InputAction rotateAction;
    private InputAction rotateTriggerAction;

    private InputAction mousePositionAction;
    private InputAction selectAction;
    private InputAction commandAction;
    private InputAction cancelAction;
    private InputAction shiftAction;

    // read input
    public Vector2 PanInput => panAction.ReadValue<Vector2>();

    public float ZoomInput => zoomAction.ReadValue<Vector2>().y;
    public float RotateInput => rotateTriggerAction.IsPressed() ? rotateAction.ReadValue<Vector2>().x : 0f;

    public bool IsRotating => rotateTriggerAction.IsPressed();
    public Vector2 MousePosition => mousePositionAction.ReadValue<Vector2>();
    public bool SelectPressed => selectAction.WasPressedThisFrame();

    public bool SelectHeld => selectAction.IsPressed();

    public bool SelectReleased => selectAction.WasReleasedThisFrame();

    public bool CommandPressed => commandAction.WasPressedThisFrame();

    public bool CancelPressed => cancelAction.WasPressedThisFrame();

    public bool ShiftHeld => shiftAction.IsPressed();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        BindActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void BindActions()
    {
        var rtsControls = inputActions.FindActionMap("Player");

        if (rtsControls == null)
        {
            Debug.LogError("Specified ActionMap not found");
            return;
        }
        
        // action bindings
        panAction = rtsControls.FindAction("Pan");
        zoomAction = rtsControls.FindAction("Zoom");
        rotateAction = rtsControls.FindAction("RotateDelta");
        rotateTriggerAction = rtsControls.FindAction("RotateTrigger");
        mousePositionAction = rtsControls.FindAction("MousePosition");
        selectAction = rtsControls.FindAction("Select");
        commandAction = rtsControls.FindAction("Command");
        cancelAction = rtsControls.FindAction("Cancel");
        shiftAction = rtsControls.FindAction("Shift");
    }
}
