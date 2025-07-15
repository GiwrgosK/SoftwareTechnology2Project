using UnityEngine;

public class CameraController : MonoBehaviour {
    private readonly float moveSpeed = 40f;
    private readonly float rotationSpeed = 100f;
    private readonly float minX = -25f;
    private readonly float maxX = 15f;
    private readonly float minZ = -30f;
    private readonly float maxZ = 10f;

    private void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);
        transform.position = position;
    }
}