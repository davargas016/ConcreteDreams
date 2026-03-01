using UnityEngine;

[CreateAssetMenu(menuName = "Game/Objectives/ReachMoney")]
public class ReachMoneyObjective : ObjectiveDefinition
{
    public int requiredMoney; // stored in cents

    public override bool IsSatisfied(PlayerInventory inv)
    {
        return inv != null && inv.money >= requiredMoney;
    }

    public override string GetProgressText(PlayerInventory inv)
    {
        if (inv == null) return "$0.00 / $0.00";

        float playerDollars = inv.money / 100f;
        float requiredDollars = requiredMoney / 100f;

        return $"${playerDollars:0.00} / ${requiredDollars:0.00}";
    }
}
