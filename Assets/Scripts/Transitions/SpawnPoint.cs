using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Unique ID for this spawn point within the scene.")]
    public string spawnId;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.75f);
    }
}