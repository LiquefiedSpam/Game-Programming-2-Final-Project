using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const int MAX_HUNGER = 100;

    [SerializeField]
    private float _speed;

    private PlayerInputController _playerInputController;
    private float _money = 0f;

    private bool _canMove = true;

    public bool CanAfford(float cost)
    {
        return _money >= cost;
    }

    void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        InteractableBehavior.OnInteract += Interact;
        InteractableBehavior.OnEndInteract += EndInteract;
    }

    void Start()
    {
        HungerManager.Ins.OnHungerKnockedOut += Knockout;
    }

    void OnDisable()
    {
        InteractableBehavior.OnInteract -= Interact;
        InteractableBehavior.OnEndInteract -= EndInteract;
        HungerManager.Ins.OnHungerKnockedOut -= Knockout;
    }

    void Update()
    {
        if (!_canMove) return;
        Debug.Log("hey");

        Vector3 positionChange = new Vector3(
            _playerInputController.MovementInputVector.x,
            0,
            _playerInputController.MovementInputVector.y)
            * Time.deltaTime
            * _speed;

        transform.position += positionChange;
    }

    void Interact(float hungerCost)
    {
    }

    void EndInteract()
    {
    }

    void Knockout()
    {
        ForceNextDay();
    }

    void ForceNextDay()
    {
        Vector3 pos = InnBehavior.LastVisitedTeleportPoint.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        HungerManager.Ins.ResetHunger();
        DayManager.Ins.NextDay();
    }

    public void SetMovementEnabled(bool enabled)
    {
        _canMove = enabled;

        if (!enabled)
        {
            _playerInputController.ResetMovement();
        }
    }
}
