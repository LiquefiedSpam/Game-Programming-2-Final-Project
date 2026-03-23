using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const int MAX_HUNGER = 10;

    [SerializeField]
    private float _speed;

    private PlayerInputController _playerInputController;
    private float _hunger;
    private float _money = 0f;

    public bool CanAfford(float cost)
    {
        return _money >= cost;
    }

    void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _hunger = MAX_HUNGER;
        InteractableBehavior.OnInteract += Interact;
        InteractableBehavior.OnEndInteract += EndInteract;
    }

    void Update()
    {
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
        _hunger -= hungerCost;
        UIManager.Ins.UpdateHungerUI(_hunger);
    }

    void EndInteract()
    {
        if (_hunger <= 0)
        {
            ForceNextDay();
            return;
        }
    }

    void ForceNextDay()
    {
        Vector3 pos = InnBehavior.LastVisitedTeleportPoint.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        _hunger = MAX_HUNGER;
        UIManager.Ins.UpdateHungerUI(_hunger);
        DayManager.Ins.NextDay();
    }
}
