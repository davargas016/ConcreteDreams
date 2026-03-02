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

    public void GiveItem(InventoryItem item, int amount = 1)
    {
        if (playerInventory == null || item == null || amount <= 0) return;

        if (!playerInventory.myInventory.Contains(item))
            playerInventory.myInventory.Add(item);

        item.numberHeld += amount;

        OnItemChanged();
    }

    public void OnMoneyChanged()
    {
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }

    public void OnItemChanged()
    {
        RecalculateAll();
        OnObjectivesChanged?.Invoke();
    }

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
        if (q.def.consumeItemsOnTurnIn && q.def.objective is CollectItemObjective collectObj)
        {
            int toConsume = (q.def.consumeAmountOverride > 0)
                ? q.def.consumeAmountOverride
                : collectObj.requiredAmount;

            if (playerInventory != null)
                playerInventory.RemoveItemByName(collectObj.itemName, toConsume);
        }

        Completed.Add(q.def.id);

        if (q.def.rewardMoney > 0 && playerInventory != null)
            {
            playerInventory.AddMoney(q.def.rewardMoney);
            itemSignal.Raise();
            }

        Active.Remove(q);

        if (q.def.nextQuest != null)
            AddQuest(q.def.nextQuest);

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


