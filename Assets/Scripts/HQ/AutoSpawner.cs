using UnityEngine;

public class AutoSpawner : MonoBehaviour {
    private Headquarters hq;

    private void Awake() {
        hq = GetComponent<Headquarters>();
    }

    private void Update() {
        if (hq != null) {
            hq.SpawnUnit();
        }
    }
}