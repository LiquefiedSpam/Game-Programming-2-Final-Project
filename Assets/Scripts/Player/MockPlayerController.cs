using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class MockPlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Animator animator;

    void Awake()
    {
        Data.MockPlayer = this;
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public async Task MoveTo(Vector3 dest)
    {

        Vector3 startPos = transform.position;
        Vector3 trueDest = new(dest.x, transform.position.y, dest.z);

        Vector3 direction = trueDest - startPos;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        float distance = Vector3.Distance(startPos, trueDest);
        float duration = distance / speed;

        SetAnimatorWalking();

        float timePassed = 0f;
        while (Vector3.Distance(transform.position, trueDest) > 0.5f)
        {
            await Task.Yield();
            timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, trueDest, timePassed / duration);
        }

        SetAnimatorIdle();
    }

    void SetAnimatorWalking()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
    }

    void SetAnimatorIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
    }
}