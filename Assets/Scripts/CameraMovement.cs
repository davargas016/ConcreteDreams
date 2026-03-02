using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 10f;
    [SerializeField] Vector3 offset;
    Rigidbody2D targetRb;

    void Awake()
    {
        if (target) targetRb = target.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = targetRb ? (Vector3)targetRb.position : target.position;
        desired += offset;
        desired.z = transform.position.z;

        float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, desired, t);
    }
}
