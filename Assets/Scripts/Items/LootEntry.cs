using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public InventoryItem item;

    [Min(0)]
    public int minAmount = 1;

    [Min(0)]
    public int maxAmount = 1;

    [Range(0f, 1f)]
    public float dropChance = 1f;

    public int RollAmount()
    {
        if (item == null) return 0;
        if (maxAmount < minAmount) maxAmount = minAmount;

        if (Random.value > dropChance)
            return 0;

        return Random.Range(minAmount, maxAmount + 1);
    }
}
