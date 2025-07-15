using UnityEngine;

public class UnitSelectable : MonoBehaviour {
    [SerializeField] private GameObject selectionIndicator;
    public UnitMover Mover;
    public UnitAttacker Attacker;

    public void SetSelected(bool selected) {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(selected);
    }
}