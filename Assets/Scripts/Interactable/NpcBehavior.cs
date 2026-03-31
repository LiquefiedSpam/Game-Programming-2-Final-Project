using UnityEngine;
using System.Collections;

public class NpcBehavior : InteractableBehavior
{
    [SerializeField] string _name;
    [TextArea(2, 10)]
    [SerializeField] Sprite _portrait;
    [SerializeField] string _dialog;

    private float detectionRadius = 10f;
    private Quaternion defaultRotation;
    private Coroutine _rotateCoroutine;

    Animator animator;
    public override InteractableType Type => InteractableType.NPC;
    public static NpcBehavior InteractingWith;


    void Awake()
    {
        defaultRotation = this.transform.rotation;
        animator = GetComponent<Animator>();
        setAnimation("isIdle");
    }
    public override void Interact(Vector3 playerPos)
    {
        if (InteractingWith != null)
        {
            Debug.LogError($"Already interacting with {InteractingWith.gameObject.name}");
            return;
        }
        base.Interact(playerPos);
        InteractingWith = this;

        //turn to face player
        animator.SetBool("isTurning", true);
        Vector3 direction = playerPos - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        StartRotate(targetRotation, "isTalking");

        //now, dialogue
        UIManager.Ins.ShowDialog(true, _name, _dialog, _portrait);
        DayManager.Ins.ConsumeUnit(1);
    }

    public override void Quit()
    {
        base.Quit();
        UIManager.Ins.ShowDialog(false);
        InteractingWith = null;

        //disengage visually
        StartRotate(defaultRotation, "isIdle");
    }



    private void StartRotate(Quaternion targetRot, string targetAnim)
    {
        if (_rotateCoroutine != null) StopCoroutine(_rotateCoroutine);
        _rotateCoroutine = StartCoroutine(RotateCoroutine(targetRot, targetAnim));
    }

    private IEnumerator RotateCoroutine(Quaternion targetRot, string targetAnim)
    {
        animator.SetBool("isTurning", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isTalking", false);

        float speed = 120f;

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                speed * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRot;
        animator.SetBool("isTurning", false);
        animator.SetBool(targetAnim, true);
    }

    void setAnimation(string animName)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isTalking", false);
        animator.SetBool("isTurning", false);

        animator.SetBool(animName, true);
    }
}
