using System;
using UnityEngine;
public class RTSCameraManager : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float edgeScrollThreshold = 10f;
    [SerializeField] private bool enableEdgeScrolling = true;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 80f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;


    [Header("Map Boundaries")]
    [SerializeField] private bool useBoundaries = true;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minZ = -50f;
    [SerializeField] private float maxZ = 50f;

    private float currentZoom;

    private RTSInputManager input;
    private bool edgeScrollActive = true;


    private void Start()
    {
        input = RTSInputManager.Instance;

        // Initialize zoom to the camera's current Y position
        currentZoom = transform.position.y;
    }


    private void Update()
    {
        if (input == null) return;

        // ESC disables edge scrolling
        if (input.CancelPressed)
            edgeScrollActive = false;
        if (input.SelectPressed)
            edgeScrollActive = true;

        HandlePanning();
        HandleEdgeScrolling();
        HandleZoom();
        HandleRotation();
        ClampPosition();
    }

    private void HandlePanning()
    {
        // get PanInput
        Vector2 panInput = input.PanInput;
        
        // calculate movement direction relative to camera orientation
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        // combine the direction vectors with input
        Vector3 movement = (forward * panInput.y + right * panInput.x) * panSpeed * Time.deltaTime;

        transform.position += movement;
    }

    private void HandleEdgeScrolling()
    {
        if (!enableEdgeScrolling || !edgeScrollActive) return;

        Vector3 movement = Vector3.zero;
        
        // get MousePosition
        Vector2 mousePos = input.MousePosition;
        
        // calculate movement
        if (mousePos.x < edgeScrollThreshold)
            movement -= transform.right; // Move left
        else if (mousePos.x > Screen.width - edgeScrollThreshold)
            movement += transform.right; // Move right

        if (mousePos.y < edgeScrollThreshold)
            movement -= transform.forward; // Move backward
        else if (mousePos.y > Screen.height - edgeScrollThreshold)
            movement += transform.forward; // Move forward

        movement.y = 0f;
        transform.position += movement.normalized * panSpeed * Time.deltaTime;

    }

    private void HandleZoom()
    {
        // get ZoomInput
        float scrollInput = input.ZoomInput;

        scrollInput = Mathf.Clamp(scrollInput/120, -1f, 1f);

        currentZoom -= scrollInput * zoomSpeed;

        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, currentZoom, Time.deltaTime * 10f);
        transform.position = pos;
    }

    private void HandleRotation()
    {
        // get RotateInput if rotating
        if (input.IsRotating)
        {
            float rotationInput = input.RotateInput;

            // apply the rotation
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.World);
        }

    }

    // to keep the camera within the defined map boundaries
    private void ClampPosition()
    {
        if (!useBoundaries) return;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}
