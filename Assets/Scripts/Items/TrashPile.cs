using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Only needed if you wire a prompt text

[RequireComponent(typeof(Collider2D))]
public class TrashPile : MonoBehaviour
{
    [Header("Loot Table")]
    public List<LootEntry> lootTable = new List<LootEntry>();

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";
    [Tooltip("Optional prompt (TMP) shown when the player is in range, e.g., 'Press E to search'")]


    [Header("Lifecycle")]
    [SerializeField] private bool destroyAfterLoot = true;
    [Tooltip("If > 0, the pile will respawn after this time instead of being permanently destroyed.")]
    [SerializeField] private float respawnSeconds = 0f;

    [Header("References")]
    [Tooltip("Your InventoryManager in the scene (used to refresh UI and access PlayerInventory).")]
    [SerializeField] private InventoryManager inventoryManager;
    [Tooltip("If not set, will try to use inventoryManager.playerInventory.")]
    [SerializeField] private PlayerInventory playerInventory;

    public Signal contextOn;
    public Signal contextOff;

    // State
    private bool playerInRange = false;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true; // This script expects trigger behavior
    }

    private void Start()
    {
        // Fallback: try to pull PlayerInventory from InventoryManager if not assigned
        if (playerInventory == null && inventoryManager != null)
        {
            playerInventory = inventoryManager.playerInventory;
        }

    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            Loot();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            contextOn.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            contextOff.Raise();
            playerInRange = false;
        }
    }

    private void Loot()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("[TrashPile] No PlayerInventory reference set or found.");
            return;
        }

        int totalFound = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var entry in lootTable)
        {
            if (entry == null || entry.item == null) continue;

            int amount = entry.RollAmount();
            if (amount <= 0) continue;

            // Respect 'unique' items: if already owned, don't add more.
            if (entry.item.unique && entry.item.numberHeld > 0)
            {
                continue;
            }

            // Add to the player's inventory (updates the ScriptableObject's numberHeld)
            int actuallyAdded = AddToInventory(entry.item, amount);
            if (actuallyAdded > 0)
            {
                totalFound += actuallyAdded;
                sb.AppendLine($"+ {actuallyAdded}x {entry.item.itemName}");
            }
        }

        // Update the inventory UI
        if (inventoryManager != null)
        {
            inventoryManager.ClearInventorySlots();
            inventoryManager.MakeInventorySlots();

            if (totalFound > 0)
            {
                // Brief feedback in the description panel
                inventoryManager.SetTextAndButton(sb.ToString(), false);
            }
            else
            {
                inventoryManager.SetTextAndButton("Nothing found.", false);
            }
        }

        // Handle lifecycle: destroy, disable, or respawn
        if (destroyAfterLoot)
        {
            if (respawnSeconds > 0f)
            {
                StartCoroutine(RespawnRoutine());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        
    }

    
    private int AddToInventory(InventoryItem item, int amount)
    {
        if (item == null || amount <= 0) return 0;

        // The InventoryManager expects that playerInventory.myInventory contains the canonical list of InventoryItems.
        // We just bump the ScriptableObject's numberHeld field.
        // (If you later add a dedicated AddItem API on PlayerInventory, call it here instead.)
        item.numberHeld += amount;

        // Unique safeguard: if unique and we overshot, clamp to 1
        if (item.unique && item.numberHeld > 1)
        {
            int overshoot = item.numberHeld - 1;
            item.numberHeld = 1;
            return amount - overshoot;
        }

        return amount;
    }

    private IEnumerator RespawnRoutine()
    {
        // Disable interaction while “gone”
        col.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(respawnSeconds);

        // Re-enable
        col.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }
}
