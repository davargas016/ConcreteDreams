using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 10f;   // higher = snappier
    [SerializeField] Vector3 offset;            // optional offset
    Rigidbody2D targetRb;

    void Awake()
    {
        if (target) targetRb = target.GetComponent<Rigidbody2D>();
    }

    // Run after everything else for the frame
    void LateUpdate()
    {
        if (!target) return;

        // Use rb.position if available to stay in sync with physics
        Vector3 desired = targetRb ? (Vector3)targetRb.position : target.position;
        desired += offset;
        desired.z = transform.position.z; // keep current Z

        // Frame-rate independent smoothing: exponential decay
        float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desired, t);
    }
}
