using UnityEngine;
public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D harvestCursor;
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
    [SerializeField] private LayerMask resourceLayer;

    private RTSInputManager input;

    void Start()
    {
        input = RTSInputManager.Instance;
    }

    void Update()
    {
       
    }
}
