using UnityEngine;
public class ShrinkEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private float startScale = 1.5f;
    [SerializeField] private float endScale = 0.2f;
    [SerializeField] private float rotationSpeed = 100f;

    private float timer;
    private Vector3 startScaleVec;
    private Vector3 endScaleVec;

    private void Start()
    {
        startScaleVec = Vector3.one * startScale;
        endScaleVec = Vector3.one * endScale;
        transform.localScale = startScaleVec;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifetime;

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        transform.localScale = Vector3.Lerp(startScaleVec, endScaleVec, t);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
