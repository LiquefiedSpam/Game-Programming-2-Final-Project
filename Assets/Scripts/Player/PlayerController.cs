using System;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public const int MAX_HUNGER = 100;

    [SerializeField] private float _speed;
    [SerializeField] private float travelSpeed = 20f;
    private Animator animator;

    private PlayerInputController _playerInputController;

    [SerializeField] private bool _canMove = true;

    public static Action<Town> OnForceNextDay;

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
        GameManager.Ins.OnEnterExitCutscene += SetMovementDisable;
    }

    void OnDisable()
    {
        InteractableBehavior.OnInteract -= Interact;
        InteractableBehavior.OnEndInteract -= EndInteract;
        GameManager.Ins.OnEnterExitCutscene -= SetMovementDisable;
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
        OnForceNextDay?.Invoke(InnBehavior.LastVisitedTown);
        MoneyManager.Ins.HalfMoney();

        Vector3 pos = InnBehavior.LastVisitedTeleportPoint.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        HungerManager.Ins.ResetHunger();
        DayManager.Ins.NextDay();
    }

    public void SetMovementDisable(bool disabled)
    {
        _canMove = !disabled;

        if (disabled)
        {
            _playerInputController.ResetMovement();
        }
    }

    public void FollowDirection(Vector3 dir)
    {
        StartCoroutine(MoveInDirection(dir));
    }

    private IEnumerator MoveInDirection(Vector3 dir)
    {
        float elapsed = 0f;
        float duration = 2f;

        //face the direction of travel and start walk anim
        transform.rotation = Quaternion.LookRotation(dir);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);

        while (elapsed < duration)
        {
            transform.position += dir * (_speed * 0.4f) * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        //return to idle when done
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
    }

    public void SetLocation(Vector3 worldPos)
    {
        transform.position = worldPos;
    }
}
