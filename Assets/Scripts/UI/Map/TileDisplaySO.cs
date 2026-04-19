using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TileDisplaySO", menuName = "Scriptable Objects/TileDisplaySO")]
public class TileDisplaySO : ScriptableObject
{
    [Header("UI")]
    [SerializeField] Vector2 normSingleInteractionPos;
    [SerializeField] Vector2 normUpInteractionPos;
    [SerializeField] Vector2 normDownInteractionPos;
    [Header("Symbols")]
    [SerializeField] Sprite unknownSprite;
    [SerializeField] Sprite safeSprite;
    [SerializeField] Sprite maraudersSprite;
    [Header("Colors")]
    [SerializeField] Color dangerColor;
    [SerializeField] Color safetyColor;
    [Header("Levels")]
    [SerializeField] Sprite level1;
    [SerializeField] Sprite level2;
    [SerializeField] Sprite level3;
    [SerializeField] Sprite level4;
    [SerializeField] Sprite level5;

    public void SetInteractionSymbols(Image symbolImg, Image levelImg, InteractionInfo interactionInfo)
    {
        if (interactionInfo.MarauderChance < 0.5f)
        {
            symbolImg.sprite = safeSprite;
            levelImg.sprite = GetLevelSprite(0.5f - interactionInfo.MarauderChance);
            levelImg.color = safetyColor;
            levelImg.gameObject.SetActive(true);
        }
        else if (interactionInfo.MarauderChance > 0.5f)
        {
            symbolImg.sprite = maraudersSprite;
            levelImg.sprite = GetLevelSprite(interactionInfo.MarauderChance - 0.5f);
            levelImg.color = dangerColor;
            levelImg.gameObject.SetActive(true);
        }
        else
        {
            symbolImg.sprite = unknownSprite;
            levelImg.gameObject.SetActive(false);
        }
    }

    public Vector2 GetUpInteractionLocalPosition(float parentWidth)
    {
        return normUpInteractionPos * parentWidth;
    }

    public Vector2 GetDownInteractionLocalPosition(float parentWidth)
    {
        return normDownInteractionPos * parentWidth;
    }

    public Vector2 GetSingleInteractionLocalPosition(float parentWidth)
    {
        return normSingleInteractionPos * parentWidth;
    }

    Sprite GetLevelSprite(float distanceFromPointFive)
    {
        return distanceFromPointFive switch
        {
            0.1f => level1,
            0.2f => level2,
            0.3f => level3,
            0.4f => level4,
            _ => level5
        };
    }
}