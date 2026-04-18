using UnityEngine;

public class MapDisplayManager : MonoBehaviour
{
    [SerializeField] GameObject root;

    public bool IsVisible => root.activeInHierarchy;

    void Start()
    {
        UIManager.Ins.OnDisplayBlocksOthers += HandleDisplayBlocksOthers;
    }

    void OnDestroy()
    {
        if (UIManager.Ins != null) UIManager.Ins.OnDisplayBlocksOthers -= HandleDisplayBlocksOthers;
    }

    public void Show()
    {
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

    void HandleDisplayBlocksOthers()
    {
        Hide();
    }
}