using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BubbleScript : MonoBehaviour
{
    Camera _cam;
    [SerializeField] Image bubble;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform heartSpawnPoint; // position near NPC head
    public bool isSpecial = false;

    Vector3 defaultScale;
    float expandSize = 1.5f;
    bool exhausted = false;

    Color defaultCol = Color.white;
    Color exhaustedCol = Color.gray;
    Color specialCol = Color.orange;
    Color exhaustedSpecialCol = new Color(0.6f, 0.3f, 0f);

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    void OnDestroy()
    {
        if (DayManager.Ins != null)
            DayManager.Ins.OnTimeChanged -= SetDefault;
    }

    void Start()
    {
        DayManager.Ins.OnDayChanged += SetDefault;
        _cam = Camera.main;

        if (isSpecial)
        {
            bubble.color = specialCol;
        }
        else
        {
            bubble.color = defaultCol;
        }
    }

    void LateUpdate()
    {
        transform.forward = _cam.transform.forward;
    }

    public bool IsExhausted()
    {
        return exhausted;
    }

    public void SetExhausted()
    {
        if (isSpecial)
        {
            bubble.color = exhaustedSpecialCol;
        }
        else
        {
            bubble.color = exhaustedCol;
        }

        exhausted = true;
    }

    public void SetDefault()
    {
        if (isSpecial)
        {
            bubble.color = specialCol;
        }
        else
        {
            bubble.color = defaultCol;
        }

        exhausted = false;
    }

    public void Expand(bool inRange)
    {
        Debug.Log("grggr");
        if (inRange)
            StartCoroutine(ScaleTo(defaultScale * expandSize, 0.1f));

        if (!inRange)
            StartCoroutine(ScaleTo(defaultScale, 0.1f));
    }

    IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public IEnumerator PopAndShrink()
    {
        Debug.Log("yeah");
        yield return ScaleTo(defaultScale * 1.8f, 0.1f); // pop bigger
        yield return ScaleTo(defaultScale, 0.2f);        // settle back to default
    }

    public IEnumerator SpawnHeart()
    {
        GameObject heartObj = Instantiate(heartPrefab, heartSpawnPoint.position, Quaternion.identity);
        SpriteRenderer heart = heartObj.GetComponent<SpriteRenderer>();

        Vector3 startPos = heartObj.transform.position;
        Vector3 endPos = startPos + new Vector3(Random.Range(-0.3f, 0.3f), 1.5f, 0f);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            heartObj.transform.position = Vector3.Lerp(startPos, endPos, t);
            heart.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));
            heartObj.transform.forward = Camera.main.transform.forward;

            yield return null;
        }

        Destroy(heartObj);
    }
}
