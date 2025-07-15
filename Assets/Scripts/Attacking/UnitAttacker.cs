using UnityEngine;

public class UnitAttacker : MonoBehaviour {
    [Header("Attack Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask obstacleMask;
    private readonly float fireRate = 1f;
    private readonly int damage = 30;

    private float _lastFireTime;
    private Health _targetHealth;
    private UnitMover _mover;
    private readonly float attackRange = 10f;
    private Health _ownHealth;

    private void Awake() {
        _mover = GetComponent<UnitMover>();
        _ownHealth = GetComponent<Health>();
        if (_ownHealth != null) {
            _ownHealth.OnDeath += HandleDeath;
        }
    }

    private void OnDestroy() {
        if (_ownHealth != null) {
            _ownHealth.OnDeath -= HandleDeath;
        }
    }

    public void SetTarget(Health target) {
        _targetHealth = target;

        if (_targetHealth != null && _mover != null) {
            _mover.MoveTo(_targetHealth.transform.position);
        }
    }

    private void Update() {
        if (_targetHealth == null || !_targetHealth.isActiveAndEnabled || _mover == null) return;

        Vector3 targetPosition = _targetHealth.transform.position;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance <= attackRange && HasLineOfSight(targetPosition)) {
            _mover.Stop();
            if (Time.time >= _lastFireTime + 1f / fireRate) {
                FireProjectile(targetPosition);
                _lastFireTime = Time.time;
            }
        } else {
            _mover.MoveTo(targetPosition);
        }
    }

    private bool HasLineOfSight(Vector3 targetPosition) {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 dir = (targetPosition - origin).normalized;
        float distance = Vector3.Distance(origin, targetPosition);

        return !Physics.Raycast(origin, dir, distance, obstacleMask);
    }

    private void FireProjectile(Vector3 targetPosition) {
        if (projectilePrefab == null || firePoint == null) {
            Debug.LogWarning("UnitAttacker is missing references.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (projScript != null) {
            projScript.Setup(targetPosition, _targetHealth, damage);
        }
    }

    private void HandleDeath() {
        _targetHealth = null;
        Destroy(gameObject);
    }
}