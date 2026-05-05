using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class DialogueOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int _timeCost;

    public void Setup(DialogueOptionInstance opt)
    {
        _timeCost = opt.definition.timeCost;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (_timeCost > 0)
                DayManager.Ins.AddToTimeUnitTally(_timeCost);
            DialogueDriver.Ins.HandleOptionSelected(opt);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_timeCost > 0)
            DayManager.Ins.PreviewUnit(true, _timeCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_timeCost > 0)
            DayManager.Ins.PreviewUnit(false);
    }
}
