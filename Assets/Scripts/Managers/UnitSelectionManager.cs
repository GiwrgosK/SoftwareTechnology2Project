using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour {
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform selectionBoxUI;
    [SerializeField] private LayerMask unitLayer;

    private Vector2 _startPos;
    private List<UnitSelectable> _selectedUnits = new();
    private bool _isSelecting = false;

    private void Update() {
        HandleSelectionInput();
        HandleCommandInput();
    }

    private void HandleSelectionInput() {
        if (Input.GetMouseButtonDown(0)) {
            _startPos = Input.mousePosition;
            _isSelecting = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            _isSelecting = false;
            Vector2 endPos = Input.mousePosition;

            if (Vector2.Distance(_startPos, endPos) < 5f) {
                SelectSingleUnitAtCursor();
            } else {
                SelectUnitsInBox();
            }
            selectionBoxUI.gameObject.SetActive(false);
        }

        if (_isSelecting) {
            UpdateSelectionBox();
        }
    }


    private void HandleCommandInput() {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if ((1 << hit.collider.gameObject.layer & unitLayer) != 0) {
                    if (hit.collider.TryGetComponent(out Health targetHealth)) {
                        foreach (var unit in _selectedUnits) {
                            unit.Attacker.SetTarget(targetHealth);
                        }
                    }
                } else {
                    foreach (var unit in _selectedUnits) {
                        unit.Attacker.SetTarget(null);
                        if (unit.Mover != null) {
                            unit.Mover.MoveTo(hit.point);  
                        }
                    }
                }
            }
        }
    }

    private void UpdateSelectionBox() {
        if (!selectionBoxUI.gameObject.activeInHierarchy)
            selectionBoxUI.gameObject.SetActive(true);

        Vector2 currentMouse = Input.mousePosition;

        float width = currentMouse.x - _startPos.x;
        float height = currentMouse.y - _startPos.y;

        selectionBoxUI.anchoredPosition = _startPos + new Vector2(
            width < 0 ? width : 0,
            height < 0 ? height : 0
        );
        selectionBoxUI.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
    }

    private void SelectUnitsInBox() {
        _selectedUnits.Clear();
        Vector2 min = selectionBoxUI.anchoredPosition;
        Vector2 max = min + selectionBoxUI.sizeDelta;

        foreach (var unit in FindObjectsByType<UnitSelectable>(FindObjectsSortMode.None)) {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(unit.transform.position);
            if (screenPos.z < 0) continue;
            if (unit.tag.Contains("Bot")) continue;

            if (screenPos.x >= min.x && screenPos.x <= max.x &&
                screenPos.y >= min.y && screenPos.y <= max.y) {
                _selectedUnits.Add(unit);
                unit.SetSelected(true);
            } else {
                unit.SetSelected(false);
            }
        }
    }

    private void SelectSingleUnitAtCursor() {
        _selectedUnits.Clear();

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, unitLayer)) {
            // Check for unit
            if (hit.collider.TryGetComponent(out UnitSelectable unit)) {
                if (unit.tag.Contains("Bot")) return;
                _selectedUnits.Add(unit);
                unit.SetSelected(true);
                FindFirstObjectByType<HQSelectable>()?.SetSelected(false);
            }
            // Check for HQ
            else if (hit.collider.TryGetComponent(out HQSelectable hq)) {
                hq.SetSelected(true);
                foreach (var test in FindObjectsByType<UnitSelectable>(FindObjectsSortMode.None)) {
                    test.SetSelected(false);
                }
            }
        } else {
            // Deselect all
            foreach (var unit in FindObjectsByType<UnitSelectable>(FindObjectsSortMode.None)) {
                unit.SetSelected(false);
            }
            FindFirstObjectByType<HQSelectable>()?.SetSelected(false);
        }
    }
}