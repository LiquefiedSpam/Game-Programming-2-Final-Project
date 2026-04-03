using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBehavior : InteractableBehavior
{
    [SerializeField] string _name;
    [TextArea(2, 10)]
    [SerializeField] List<DialogueUnit> beginningDialogue;
    [SerializeField] string _dialog;
    [SerializeField] Sprite _portrait;


    private float detectionRadius = 10f;
    private Quaternion defaultRotation;
    private Coroutine _rotateCoroutine;

    Animator animator;

    public override InteractableType Type => InteractableType.DIALOG;

    public static DialogBehavior InteractingWith;

    void Awake()
    {
        //beginningDialogue = new List<DialogueUnit>();
        defaultRotation = this.transform.rotation;
        animator = GetComponent<Animator>();
        animator.SetBool("isIdle", true);
        animator.SetBool("isTalking", false);
        animator.SetBool("isTurning", false);
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
}
