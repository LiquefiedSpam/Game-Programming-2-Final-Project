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
        if (!isActiveAndEnabled)
            return;

        Vector3 target = inRange ? defaultScale * expandSize : defaultScale;
        StartCoroutine(UIAnimations.ScaleTo(transform, target, 0.1f));
    }

    public IEnumerator PopAndShrink()
    {
        yield return UIAnimations.PopAndShrink(transform, defaultScale, 1.6f);
    }

    public void SpawnHeart()
    {
        GameObject heartObj = Instantiate(heartPrefab, heartSpawnPoint.position, Quaternion.identity);
        SpriteRenderer heart = heartObj.GetComponent<SpriteRenderer>();
        StartCoroutine(UIAnimations.FloatUpAndFade(heart));
    }
}
