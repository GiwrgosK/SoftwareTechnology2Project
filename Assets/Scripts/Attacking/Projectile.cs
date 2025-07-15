using UnityEngine;

public class Projectile : MonoBehaviour {
    private Vector3 targetPosition;
    private Health targetHealth;
    private int damage;

    private readonly float moveSpeed = 10f;

    public void Setup(Vector3 targetPosition, Health targetHealth, int damage) {
        this.targetPosition = targetPosition;
        this.targetHealth = targetHealth;
        this.damage = damage;
    }

    private void Update() {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
        if (distanceBeforeMoving < distanceAfterMoving) {
            transform.position = targetPosition;

            if (targetHealth != null) {
                targetHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}