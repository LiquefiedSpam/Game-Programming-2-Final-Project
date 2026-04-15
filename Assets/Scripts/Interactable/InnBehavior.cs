using UnityEngine;

public class InnBehavior : InteractableBehavior
{
    [SerializeField] private Transform _teleportPoint;
    [SerializeField] private bool _defaultLastVisited;
    [SerializeField] private Town _town;

    public override InteractableType Type => InteractableType.INN;
    public override bool Instant => true;
    public static Transform LastVisitedTeleportPoint;
    public static Town LastVisitedTown;

    void Awake()
    {
        if (_defaultLastVisited)
        {
            LastVisitedTeleportPoint = _teleportPoint;
            LastVisitedTown = _town;
        }
    }

    public override void Interact()
    {
        base.Interact();
        LastVisitedTeleportPoint = _teleportPoint;
        LastVisitedTown = _town;
        DayManager.Ins.NextDay();
    }
}