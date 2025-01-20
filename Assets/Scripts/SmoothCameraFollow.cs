using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;

    private Vector3 vel = Vector3.zero;

    private void FixedUpdate() {
        Vector3 targetPosition = target.position + offset;

        // Ensure the camera's Z position remains constant
        targetPosition.z = transform.position.z;

        // Calculate new position using SmoothDamp
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);

        Debug.Log("Camera Position: " + transform.position);
        Debug.Log("Target Position: " + target.position);
        Debug.Log("New Camera Position: " + newPosition);

        // Clamp the camera position to ensure it stays within certain bounds (optional)
        // newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        // newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}
