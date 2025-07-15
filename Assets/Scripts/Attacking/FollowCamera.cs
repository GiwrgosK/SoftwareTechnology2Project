using UnityEngine;

public class FollowCamera : MonoBehaviour {
    [SerializeField] private bool invert;
    
    private new Transform camera;

    private void Awake() {
        camera = Camera.main.transform;
    }

    private void LateUpdate() {
        if (invert) {
            Vector3 directionToCamera = (camera.position - transform.position).normalized;
            transform.LookAt(transform.position + directionToCamera * -1);
        } else {
            transform.LookAt(camera.transform);
        }
    }
}