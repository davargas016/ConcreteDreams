using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishingLootEntry
{
    public InventoryItem item;

    [Min(1)] public int minAmount = 1;
    [Min(1)] public int maxAmount = 1;

    [Min(0)] public int weight = 1;
}

[CreateAssetMenu(fileName = "FishingLootTable", menuName = "Fishing/Fishing Loot Table")]
public class FishingLootTable : ScriptableObject
{
    public List<FishingLootEntry> entries = new List<FishingLootEntry>();

    public bool TryRoll(out InventoryItem item, out int amount)
    {
        item = null;
        amount = 0;

        int totalWeight = 0;
        foreach (var e in entries)
        {
            if (e == null || e.item == null) continue;
            if (e.weight <= 0) continue;
            totalWeight += e.weight;
        }

        if (totalWeight <= 0) return false;

        int roll = Random.Range(1, totalWeight + 1);
        int running = 0;

        foreach (var e in entries)
        {
            if (e == null || e.item == null) continue;
            if (e.weight <= 0) continue;

            running += e.weight;
            if (roll <= running)
            {
                int min = Mathf.Max(1, e.minAmount);
                int max = Mathf.Max(min, e.maxAmount);
                item = e.item;
                amount = Random.Range(min, max + 1);
                return true;
            }
        }

        return false;
    }
}
