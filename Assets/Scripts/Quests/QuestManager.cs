using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager I { get; private set; }

    [Header("Assign")]
    public PlayerInventory playerInventory;

    public List<QuestInstance> Active = new List<QuestInstance>();
    public HashSet<string> Completed = new HashSet<string>();

    public event Action OnObjectivesChanged;
    public Signal itemSignal;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
    }

    public bool IsQuestActive(string id)
    {
        return Active.Exists(q => q != null && q.def != null && q.def.id == id);
    }

    public bool IsQuestCompleted(string id)
    {
        return Completed.Contains(id);
    }

    public void AddQuest(QuestDefinition quest)
    {
        if (quest == null) return;
        if (IsQuestActive(quest.id) || IsQuestCompleted(quest.id)) return;

        QuestInstance instance = new QuestInstance(quest);
        Active.Add(instance);

        Recalculate(instance);
        OnObjectivesChanged?.Invoke();
    }

    void Recalculate(QuestInstance q)
    {
        if (q?.def?.objective == null || playerInventory == null) return;
        q.objectiveDone = q.def.objective.IsSatisfied(playerInventory);
    }

    void RecalculateAll()
    {
        foreach (var q in Active)
            Recalculate(q);
    }

    // -------------------------
    // IMPORTANT: CENTRAL METHODS
    // -------------------------

    /// <summary>
    /// Call this whenever an item is gained (flyer from dialogue, fish from fishing, etc.).
    /// This updates inventory AND refreshes objectives + UI.
    /// </summary>
    public void GiveItem(InventoryItem item, int amount = 1)
    {
        if (playerInventory == null || item == null || amount <= 0) return;

        if (!playerInventory.myInventory.Contains(item))
            playerInventory.myInventory.Add(item);

        item.numberHeld += amount;

        OnItemChanged(); // recompute + UI update
    }

    /// <summary>
    /// Call this whenever money changes.
    /// </summary>
    public void OnMoneyChanged()
    {
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }

    /// <summary>
    /// Call this whenever inventory changes (item gained/removed) if you updated inventory elsewhere.
    /// </summary>
    public void OnItemChanged()
    {
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }

    // -------------------------
    // NPC TURN-IN
    // -------------------------
    public void OnTalkToNpc(string npcId)
    {
        RecalculateAll();

        for (int i = Active.Count - 1; i >= 0; i--)
        {
            var q = Active[i];
            if (q != null && q.objectiveDone && q.def.turnInNpcId == npcId)
            {
                TurnIn(q);
            }
        }

        OnObjectivesChanged?.Invoke();
    }

    void TurnIn(QuestInstance q)
    {
        // Consume items only if quest says so AND objective is CollectItemObjective
        if (q.def.consumeItemsOnTurnIn && q.def.objective is CollectItemObjective collectObj)
        {
            int toConsume = (q.def.consumeAmountOverride > 0)
                ? q.def.consumeAmountOverride
                : collectObj.requiredAmount;

            if (playerInventory != null)
                playerInventory.RemoveItemByName(collectObj.itemName, toConsume);
        }

        // Mark completed
        Completed.Add(q.def.id);

        // Reward
        if (q.def.rewardMoney > 0 && playerInventory != null)
            {
            playerInventory.AddMoney(q.def.rewardMoney);
            itemSignal.Raise();
            }

        // Remove from active list (objective disappears from UI)
        Active.Remove(q);

        // Start chain
        if (q.def.nextQuest != null)
            AddQuest(q.def.nextQuest);

        // Refresh objectives/UI
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }


    void OnEnable()
    {
        if (playerInventory != null)
            playerInventory.Changed += HandleInventoryChanged;
    }

    void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.Changed -= HandleInventoryChanged;
    }

    void HandleInventoryChanged()
    {
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }

}


