using UnityEngine;

public class HQSelectable : MonoBehaviour {
    private void Start() {
        SetSelected(false);
    }

    public void SetSelected(bool selected) {
        if (selected) {
            HQUI.Instance.Show(this);
        } else {
            HQUI.Instance.Hide();
        }
    }

    public void SpawnUnit() {
        GetComponent<Headquarters>()?.SpawnUnit();
    }
}