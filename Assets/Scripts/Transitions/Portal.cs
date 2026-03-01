using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
    public string targetSpawnId;
    public GameObject zoneToEnable;
    public GameObject zoneToDisable;

    public ScreenFader fader;
    public float fadeOutTime = 0.2f;
    public float fadeInTime = 0.2f;

    public PlayerInventory playerInventory;
    public bool requireItem = false;
    public string requiredItemName;
    public int requiredAmount = 1;
    public bool consumeRequiredItem = false;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (requireItem)
        {
            int count = playerInventory.GetItemCount(requiredItemName);
            if (count < requiredAmount)
            {
                return;
            }

            if (consumeRequiredItem)
            {
                bool removed = playerInventory.RemoveItemByName(requiredItemName, requiredAmount);
                if (!removed)
                {
                    return;
                }
            }
        }
        StartCoroutine(TransitionRoutine(other.transform));
    }

    private IEnumerator TransitionRoutine(Transform player)
    {

        if (fader != null) yield return fader.FadeTo(1f, fadeOutTime);

        // teleport
        SpawnPoint target = FindSpawnPoint(targetSpawnId);
        if (target != null) player.position = target.transform.position;
        else Debug.LogWarning($"SpawnPoint '{targetSpawnId}' not found.");

        // toggle zones
        if (zoneToEnable != null) zoneToEnable.SetActive(true);
        if (zoneToDisable != null) zoneToDisable.SetActive(false);

        // After enabling/disabling zones:
        if (zoneToEnable != null)
        {
            var zoneInfo = zoneToEnable.GetComponent<FishingZoneInfo>();
            if (zoneInfo != null)
            {
                var fishing = player.GetComponent<FishingController>();
                if (fishing != null) fishing.SetCurrentZone(zoneInfo);
            }
        }

        if (fader != null) yield return fader.FadeTo(0f, fadeInTime);
    }

    private SpawnPoint FindSpawnPoint(string id)
    {
        foreach (var sp in FindObjectsOfType<SpawnPoint>())
            if (sp.spawnId == id) return sp;
        return null;
    }
}