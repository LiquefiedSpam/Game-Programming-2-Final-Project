using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const int MAX_HUNGER = 100;

    [SerializeField] private float _speed;
    private Animator animator;

    private PlayerInputController _playerInputController;

    private bool _canMove = true;


    void Awake()
    {
        animator = GetComponent<Animator>();
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
        if (HungerManager.Ins != null)
            HungerManager.Ins.OnHungerKnockedOut -= Knockout;
    }

    void Update()
    {
        if (!_canMove) return;

        Vector3 moveDir = new Vector3(
            _playerInputController.MovementInputVector.x,
            0,
            _playerInputController.MovementInputVector.y);

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdle", true);
        }

        transform.position += moveDir * Time.deltaTime * _speed;
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
