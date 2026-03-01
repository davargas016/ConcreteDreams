using UnityEngine;

[CreateAssetMenu(menuName = "Game/Objectives/CollectItem")]
public class CollectItemObjective : ObjectiveDefinition
{
    public string itemName;
    public int requiredAmount;

    public override bool IsSatisfied(PlayerInventory inv)
    {
        return inv != null && inv.GetItemCount(itemName) >= requiredAmount;
    }

    public override string GetProgressText(PlayerInventory inv)
    {
        if (inv == null) return $"0/{requiredAmount}";
        return $"{inv.GetItemCount(itemName)}/{requiredAmount}";
    }
}
