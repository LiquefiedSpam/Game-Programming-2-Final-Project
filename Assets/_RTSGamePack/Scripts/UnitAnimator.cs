using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Unit))]
public class UnitAnimator : MonoBehaviour
{
    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int IsActionParam = Animator.StringToHash("IsAction");
    private static readonly int IsDeadParam = Animator.StringToHash("IsDead");

    public Animator animator;
    private NavMeshAgent agent;
    private Unit unit;

    private bool wasDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (animator == null) return;

        UpdateMovementAnimation();
        UpdateDeathState();
    }
    private void UpdateMovementAnimation()
    {
    }

    private void UpdateDeathState()
    {

    }

    public void SetActionState(bool isActive)
    {

    }


    // helper
    public float GetAnimationLength(string clipName)
    {
        if (animator == null) return 0f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        return 0f;
    }
}
