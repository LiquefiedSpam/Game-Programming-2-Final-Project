using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    [Header("Layer Masks for Selectables")]
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask resourceLayer;

    [Header("Drop-off")]
    [SerializeField] private Transform dropOffPoint;
    [SerializeField] private float dragThreshold = 10f;

    [Header("Selection Box Visual")]
    [SerializeField] private RectTransform selectionBoxImage;
    [SerializeField] private GameObject moveMarkerPrefab;

    private List<Unit> selectedUnits = new List<Unit>();

    private Vector2 dragStartPosition;
    private bool isDragging;

    private RTSInputManager input;

    public List<Unit> SelectedUnits => selectedUnits;

    void Start()
    {
        input = RTSInputManager.Instance;

    }

    void Update()
    {
        if (input == null) return;

        HandleSelectionInput();
        HandleCommandInput();
    }

    void HandleSelectionInput()
    {

    }

    void HandleCommandInput()
    {

    }
    void PerformSingleSelection()
    {
    }
    void PerformBoxSelection()
    {
        
    }
    void UpdateSelectionBox()
    {
        
    }

    void SelectUnit(Unit unit)
    {

    }
    public void DeselectAll()
    {
      
    }
}
