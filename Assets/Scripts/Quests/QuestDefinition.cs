using UnityEngine;

[CreateAssetMenu(menuName = "Game/Quest")]
public class QuestDefinition : ScriptableObject
{
    public string id;
    public ObjectiveDefinition objective;
    public string turnInNpcId;
    public QuestDefinition nextQuest;
    public int rewardMoney;

    [Header("Turn-in Behavior (optional)")]
    [Tooltip("If true and the objective is CollectItemObjective, items will be removed from inventory when turned in.")]
    public bool consumeItemsOnTurnIn = false;

    [Tooltip("If > 0, use this amount to consume instead of the objective's requiredAmount.")]
    public int consumeAmountOverride = 0;
}
