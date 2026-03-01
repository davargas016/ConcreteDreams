using System.Collections;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class BranchingDialogueController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject branchingCanvas;
    [SerializeField] private GameObject dialoguePrefab;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private GameObject dialogueHolder;
    [SerializeField] private GameObject choiceHolder;
    [SerializeField] private ScrollRect dialogueScroll;

    [Header("Dialogue")]
    [SerializeField] private TextAssetValue dialogueValue;

    [Header("Unlock System")]
    [SerializeField] private DialogueUnlockRegistry unlockRegistry;

    [Header("Inventory")]
    [SerializeField] private PlayerInventory playerInventory;

    // NEW: Drag ALL possible InventoryItem assets here (Health Flyer, Quest Item, etc.)
    [SerializeField] private List<InventoryItem> allItems = new List<InventoryItem>();

    private Story myStory;

    public void EnableCanvas()
    {
        if (branchingCanvas != null)
            branchingCanvas.SetActive(true);

        SetStory();
        RefreshView();
    }

    public void SetStory()
    {
        if (dialogueValue == null)
        {
            Debug.LogWarning("DialogueValue is not assigned.");
            return;
        }

        if (dialogueValue.value)
        {
            DeleteOldDialogs();
            myStory = new Story(dialogueValue.value.text);
        }
        else
        {
            Debug.LogWarning("DialogueValue has no TextAsset assigned.");
        }
    }

    void DeleteOldDialogs()
    {
        if (dialogueHolder == null) return;

        for (int i = 0; i < dialogueHolder.transform.childCount; i++)
        {
            Destroy(dialogueHolder.transform.GetChild(i).gameObject);
        }
    }

    public void RefreshView()
    {
        if (myStory == null)
        {
            Debug.LogWarning("Story is null. Did SetStory() fail?");
            return;
        }

        while (myStory.canContinue)
        {
            string newLine = myStory.Continue();
            MakeNewDialogue(newLine);

            // process tags emitted by this line
            HandleTags(myStory.currentTags);
        }

        if (myStory.currentChoices.Count > 0)
        {
            MakeNewChoices();
        }
        else
        {
            if (branchingCanvas != null)
                branchingCanvas.SetActive(false);
        }

        StartCoroutine(ScrollCo());
    }

    private void HandleTags(List<string> tags)
    {
        if (tags == null || tags.Count == 0) return;

        foreach (var raw in tags)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            string tag = raw.Trim();

            // ---------- UNLOCK ----------
            if (tag.StartsWith("unlock:", System.StringComparison.OrdinalIgnoreCase))
            {
                if (unlockRegistry == null)
                {
                    Debug.LogWarning("Unlock tag found but unlockRegistry not assigned.");
                    continue;
                }

                string key = tag.Substring("unlock:".Length).Trim();
                unlockRegistry.Unlock(key);
                Debug.Log($"Unlocked NPC: {key}");
            }

            // ---------- LOCK ----------
            else if (tag.StartsWith("lock:", System.StringComparison.OrdinalIgnoreCase))
            {
                if (unlockRegistry == null)
                {
                    Debug.LogWarning("Lock tag found but unlockRegistry not assigned.");
                    continue;
                }

                string key = tag.Substring("lock:".Length).Trim();
                unlockRegistry.Lock(key);
                Debug.Log($"Locked NPC: {key}");
            }

            // ---------- GIVE ITEM ----------
            else if (tag.StartsWith("give_item:", System.StringComparison.OrdinalIgnoreCase))
            {
                string itemName = tag.Substring("give_item:".Length).Trim();

                if (playerInventory == null)
                {
                    Debug.LogWarning("PlayerInventory not assigned on BranchingDialogueController.");
                    continue;
                }

                if (allItems == null || allItems.Count == 0)
                {
                    Debug.LogWarning("AllItems list is empty on BranchingDialogueController. Drag your InventoryItem assets into it.");
                    continue;
                }

                InventoryItem foundItem = null;

                // Look up the InventoryItem from the master list (NOT the player's current inventory)
                foreach (var item in allItems)
                {
                    if (item != null && item.itemName == itemName)
                    {
                        foundItem = item;
                        break;
                    }
                }

                if (foundItem == null)
                {
                    Debug.LogWarning($"No InventoryItem found in AllItems with name '{itemName}'. Make sure it matches InventoryItem.itemName exactly.");
                    continue;
                }

                playerInventory.AddInventoryItem(foundItem, 1);
                Debug.Log($"Gave item: {itemName}");
            }
        }
    }

    IEnumerator ScrollCo()
    {
        yield return null;

        if (dialogueScroll != null)
            dialogueScroll.verticalNormalizedPosition = 0f;
    }

    void MakeNewDialogue(string newDialogue)
    {
        if (dialoguePrefab == null || dialogueHolder == null) return;

        DialogueObject newDialogueObject =
            Instantiate(dialoguePrefab, dialogueHolder.transform).GetComponent<DialogueObject>();

        newDialogueObject.Setup(newDialogue);
    }

    void MakeNewResponse(string newDialogue, int choiceValue)
    {
        if (choicePrefab == null || choiceHolder == null) return;

        ResponseObject newResponseObject =
            Instantiate(choicePrefab, choiceHolder.transform).GetComponent<ResponseObject>();

        newResponseObject.Setup(newDialogue, choiceValue);

        Button responseButton = newResponseObject.gameObject.GetComponent<Button>();
        if (responseButton)
        {
            responseButton.onClick.RemoveAllListeners();
            responseButton.onClick.AddListener(() => ChooseChoice(choiceValue));
        }
    }

    void MakeNewChoices()
    {
        if (choiceHolder == null) return;

        for (int i = 0; i < choiceHolder.transform.childCount; i++)
        {
            Destroy(choiceHolder.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < myStory.currentChoices.Count; i++)
        {
            MakeNewResponse(myStory.currentChoices[i].text, i);
        }
    }

    void ChooseChoice(int choice)
    {
        myStory.ChooseChoiceIndex(choice);
        RefreshView();
    }
}