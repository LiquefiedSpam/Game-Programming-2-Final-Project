using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TileDisplaySO", menuName = "Scriptable Objects/TileDisplaySO")]
public class TileDisplaySO : ScriptableObject
{
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
        if (interactionInfo.MarauderChance < 5)
        {
            symbolImg.sprite = safeSprite;
            levelImg.sprite = GetLevelSprite(5 - interactionInfo.MarauderChance);
            levelImg.color = safetyColor;
            levelImg.gameObject.SetActive(true);
        }
        else if (interactionInfo.MarauderChance > 5)
        {
            symbolImg.sprite = maraudersSprite;
            levelImg.sprite = GetLevelSprite(interactionInfo.MarauderChance - 5);
            levelImg.color = dangerColor;
            levelImg.gameObject.SetActive(true);
        }
        else
        {
            symbolImg.sprite = unknownSprite;
            levelImg.gameObject.SetActive(false);
        }
    }

    Sprite GetLevelSprite(int level)
    {
        return level switch
        {
            1 => level1,
            2 => level2,
            3 => level3,
            4 => level4,
            5 => level5,
            _ => unknownSprite
        };
    }
}