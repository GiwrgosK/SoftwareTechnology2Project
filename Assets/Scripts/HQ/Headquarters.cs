using UnityEngine;

public class Headquarters : MonoBehaviour {
    [Header("HQ Settings")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform spawnPoint;
    
    private Health health;
    private readonly float spawnCooldown = 5f;
    private float _lastSpawnTime;

    public static event System.Action<Headquarters> OnHQDestroyed;

    private void Awake() {
        health = GetComponent<Health>();
        if (health != null) {
            health.OnDeath += Health_OnDeath;
        }
    }

    private void OnDestroy() {
        health.OnDeath -= Health_OnDeath;
    }

    private void Health_OnDeath() {
        OnHQDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public void SpawnUnit() {
        if (unitPrefab == null || spawnPoint == null) return;

        if (Time.time - _lastSpawnTime >= spawnCooldown) {
            Vector3 rawSpawnPos = spawnPoint.position + Vector3.up * 2f;
            Vector3 finalPos = rawSpawnPos;

            if (Physics.Raycast(rawSpawnPos, Vector3.down, out RaycastHit hit, 10f)) {
                finalPos = hit.point;
            } else {
                Debug.LogWarning("Could not find ground beneath spawn point. Using raw position.");
            }

            GameObject unitGO = Instantiate(unitPrefab, finalPos, Quaternion.identity);

            if (unitGO.TryGetComponent(out UnityEngine.AI.NavMeshAgent agent)) {
                agent.Warp(finalPos);
            }
            _lastSpawnTime = Time.time;
        }
    }
}