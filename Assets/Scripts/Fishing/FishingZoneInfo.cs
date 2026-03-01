// FishingZoneInfo.cs
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingZoneInfo : MonoBehaviour
{
    [Header("Water Detection")]
    public Tilemap waterTilemap;

    [Header("Fishing Loot")]
    public FishingLootTable lootTable;

    [Header("Optional")]
    public string zoneName = "Zone";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        FishingController fc = other.GetComponent<FishingController>();
        if (fc != null)
        {
            fc.SetCurrentZone(this); // 'this' is FishingZoneInfo -> correct type
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        FishingController fc = other.GetComponent<FishingController>();
        if (fc != null && fc.currentZone == this)
        {
            fc.SetCurrentZone(null);
        }
    }
}