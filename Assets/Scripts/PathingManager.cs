using UnityEngine;

public class PathingManager : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;

    [Header("Path Settings")]
    [SerializeField] private GameObject topPath;
    [SerializeField] private GameObject bottomPath;

    private GameObject selectedPath;

    public bool HasSelectedPath => selectedPath != null;

    public void spawnSign()
    {

    }

    public void selectTopPath()
    {
        selectedPath = topPath;
    }

    public void selectBottomPath()
    {
        selectedPath = bottomPath;
    }

    public GameObject getSelectedPath()
    {
        return selectedPath;
    }

    public void ResetSelection()
    {
        selectedPath = null;
    }
}
