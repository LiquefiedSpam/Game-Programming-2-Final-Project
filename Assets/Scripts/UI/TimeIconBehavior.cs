using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TimeIconBehavior : MonoBehaviour
{


    public Image timeIntervalImage;
    public Vector3 fadeInStartRot = new Vector3(0, 0, 50);
    public Vector3 fadeOutEndRot = new Vector3(0, 0, -50);
    public Vector3 neutralRot = new Vector3(0, 0, 0);

    void Start()
    {
        transform.rotation = Quaternion.Euler(fadeInStartRot);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSprite(Sprite spr)
    {
        timeIntervalImage.sprite = spr;
    }

    public IEnumerator Fade(bool fadeIn)
    {
        Vector3 targetRot;
        Vector3 startRot;
        if (fadeIn)
        {
            targetRot = neutralRot;
            startRot = fadeInStartRot;
        }
        else
        {
            targetRot = fadeOutEndRot;
            startRot = neutralRot;
        }

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, targetRot, t));
            timeIntervalImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));

            yield return null;
        }

        if (!fadeIn)
            Destroy(this);
    }
}
