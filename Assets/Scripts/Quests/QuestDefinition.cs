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
    public bool consumeItemsOnTurnIn = false;

    [Tooltip("If > 0, will consume amount entered")]
    public int consumeAmountOverride = 0;
}
