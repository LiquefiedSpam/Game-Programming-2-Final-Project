using UnityEngine;
using UnityEngine.UI;

public class TileUI : MonoBehaviour
{
    [SerializeField] Image tileImage;
    [SerializeField] InteractionPointUI upPoint;
    [SerializeField] InteractionPointUI downPoint;
    [SerializeField] RectTransform parentRectTransform;

    public void ShowTile(TileInfo tileInfo)
    {
        tileImage.sprite = tileInfo.Data.Sprite;

        float width = parentRectTransform.rect.width;
        Vector2 halfWidth = new(width / 2f, width / 2f);

        Data.TileDisplaySO.SetInteractionSymbols(upPoint.symbolImage, upPoint.levelImage, tileInfo.UpInteractionInfo);
        upPoint.parent.localPosition = (tileInfo.Data.NormalizedUpPoint * width) - halfWidth;
        upPoint.parent.gameObject.SetActive(true);

        if (tileInfo.Data.ChoiceTile)
        {
            Data.TileDisplaySO.SetInteractionSymbols(downPoint.symbolImage, downPoint.levelImage, tileInfo.DownInteractionInfo);
            downPoint.parent.localPosition = (tileInfo.Data.NormalizedDownPoint * width) - halfWidth;
            downPoint.parent.gameObject.SetActive(true);
        }
        else
        {
            downPoint.parent.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public struct InteractionPointUI
{
    [SerializeField] public Image symbolImage;
    [SerializeField] public Image levelImage;
    [SerializeField] public RectTransform parent;
}
