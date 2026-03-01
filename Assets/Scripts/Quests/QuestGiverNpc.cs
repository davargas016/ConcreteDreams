using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuestGiverNpc : MonoBehaviour
{
    public string npcId;
    public QuestDefinition questToOffer;

    public Signal contextOn;
    public Signal contextOff;

    [Header("Quest Lock")]
    [SerializeField] private BoolValue unlockedFlag;

    bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {

            Debug.Log($"[QuestGiverNpc] Pressed E on NPC: {npcId}");
            if (unlockedFlag == null || unlockedFlag.value)
            {   
                if (QuestManager.I == null)
                {
                    Debug.LogError("[QuestGiverNpc] QuestManager.I is NULL. Add QuestManager to scene.");
                    return;
                }

                if (questToOffer == null)
                {
                    Debug.LogError("[QuestGiverNpc] questToOffer is NULL. Assign a QuestDefinition in Inspector.");
                }
                else
                {
                    Debug.Log($"[QuestGiverNpc] Attempting to offer quest: {questToOffer.id}");

                    if (!QuestManager.I.IsQuestActive(questToOffer.id) &&
                        !QuestManager.I.IsQuestCompleted(questToOffer.id))
                    {
                        QuestManager.I.AddQuest(questToOffer);
                        Debug.Log($"[QuestGiverNpc] Quest added: {questToOffer.id}");
                    }
                    else
                    {
                        Debug.Log($"[QuestGiverNpc] Quest already active or completed: {questToOffer.id}");
                    }
                }

                QuestManager.I.OnTalkToNpc(npcId);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Optional: only show prompt if unlocked
            if (unlockedFlag == null || unlockedFlag.value) contextOn?.Raise();

            Debug.Log($"[QuestGiverNpc] Player entered range of {npcId}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            contextOff?.Raise();
            playerInRange = false;
            Debug.Log($"[QuestGiverNpc] Player left range of {npcId}");
        }
    }
}