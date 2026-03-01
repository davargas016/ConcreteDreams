using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public enum ShopMode { Buy, Sell }

    [Header("Player Inventory")]
    public PlayerInventory playerInventory;

    [Header("UI References")]
    [SerializeField] private GameObject blankShopSlotPrefab;
    [SerializeField] private Transform shopPanel;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite defaultImage;

    [Header("Buttons")]
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI actionButtonText;

    [Header("Signals")]
    public Signal moneySignal;

    [Header("Runtime")]
    public Vendor currentVendor;
    public ShopMode mode = ShopMode.Buy;
    public InventoryItem currentItem;

    // Called by ShopToggle when opening with a specific NPC
    public void OpenVendor(Vendor vendor)
    {
        currentVendor = vendor;
        mode = ShopMode.Buy;
        RefreshShopUI();
    }

    private void OnEnable()
    {
        RefreshShopUI();
    }

    // Hook these to your UI tab buttons
    public void BuyTabPressed()
    {
        mode = ShopMode.Buy;
        RefreshShopUI();
    }

    public void SellTabPressed()
    {
        mode = ShopMode.Sell;
        RefreshShopUI();
    }

    private void RefreshShopUI()
    {
        ClearShopSlots();
        MakeShopSlots();
        ResetDetails();
        UpdateActionButtonVisual();
    }

    private void ResetDetails()
    {
        currentItem = null;
        descriptionText.text = "";
        itemNameText.text = "";
        priceText.text = "";
        itemImage.sprite = defaultImage;

        if (actionButton != null) actionButton.interactable = false;
    }

    private void UpdateActionButtonVisual()
    {
        if (actionButtonText != null)
            actionButtonText.text = (mode == ShopMode.Buy) ? "BUY" : "SELL";
    }

    public void MakeShopSlots()
    {
        if (blankShopSlotPrefab == null || shopPanel == null) return;

        if (mode == ShopMode.Buy)
        {
            if (currentVendor == null || currentVendor.itemsForSale == null) return;

            for (int i = 0; i < currentVendor.itemsForSale.Count; i++)
            {
                InventoryItem item = currentVendor.itemsForSale[i];
                if (item == null) continue;

                GameObject temp = Instantiate(blankShopSlotPrefab, shopPanel);
                ShopSlot slot = temp.GetComponent<ShopSlot>();
                if (slot != null) slot.Setup(item, this);
            }
        }
        else // SELL mode
        {
            if (playerInventory == null || playerInventory.myInventory == null) return;

            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                InventoryItem item = playerInventory.myInventory[i];
                if (item == null) continue;

                if (item.numberHeld <= 0) continue;
                if (!VendorWillBuy(item)) continue;

                GameObject temp = Instantiate(blankShopSlotPrefab, shopPanel);
                ShopSlot slot = temp.GetComponent<ShopSlot>();
                if (slot != null) slot.Setup(item, this);
            }
        }
    }

    // The key rule-set for two vendors
    private bool VendorWillBuy(InventoryItem item)
    {
        if (!item.sellable) return false;

        if (currentVendor == null) return true;

        if (currentVendor.vendorType == Vendor.VendorType.General)
        {
            return true;
        }

        if (currentVendor.vendorType == Vendor.VendorType.Fish)
        {
            return item.category == ItemCategory.Fish;
        }

        return false;
    }

    public void ClearShopSlots()
    {
        if (shopPanel == null) return;

        for (int i = shopPanel.childCount - 1; i >= 0; i--)
            Destroy(shopPanel.GetChild(i).gameObject);
    }

    public void SetupDetailsFromSlot(InventoryItem item, Sprite slotSprite)
    {
        currentItem = item;

        itemNameText.text = item.itemName;
        descriptionText.text = item.itemDescription;

        int displayPrice = (mode == ShopMode.Buy) ? item.priceInCents : GetSellPrice(item);
        priceText.text = FormatCents(displayPrice);

        itemImage.sprite = slotSprite != null ? slotSprite : item.itemImage;

        if (actionButton != null) actionButton.interactable = true;
        UpdateActionButtonVisual();
    }

    // Hook your Action button OnClick to this
    public void ActionButtonPressed()
    {
        if (currentItem == null || playerInventory == null) return;

        if (mode == ShopMode.Buy) BuySelected();
        else SellSelected();
    }

    // -----------------------------
    // FIX: Notify quests + UI
    // -----------------------------
    private void NotifyQuestSystemAfterInventoryOrMoneyChange()
    {
        if (QuestManager.I == null) return;

        // Money objectives (like Collect Money) rely on this
        QuestManager.I.OnMoneyChanged();

        // Item objectives (like Collect Flyer / Catch Fish) rely on this
        QuestManager.I.OnItemChanged();
    }

    private void BuySelected()
    {
        int cost = currentItem.priceInCents;

        if (playerInventory.money < cost) return;
        if (currentItem.unique && PlayerHasItem(currentItem)) return;

        // Money change
        playerInventory.money -= cost;
        if (moneySignal != null) moneySignal.Raise();

        // Item change
        AddItemToPlayerInventory(currentItem, 1);

        // ✅ FIX: Update quests + quest UI immediately
        NotifyQuestSystemAfterInventoryOrMoneyChange();

    }

    private void SellSelected()
    {
        if (currentItem.numberHeld <= 0) return;
        if (!VendorWillBuy(currentItem)) return;

        int payout = GetSellPrice(currentItem);

        // Item change
        currentItem.numberHeld -= 1;
        if (currentItem.numberHeld < 0) currentItem.numberHeld = 0;

        // Money change
        playerInventory.money += payout;
        if (moneySignal != null) moneySignal.Raise();

        // ✅ FIX: Update quests + quest UI immediately
        NotifyQuestSystemAfterInventoryOrMoneyChange();

        // Refresh sell list so items vanish at 0 and vendor filtering applies
        RefreshShopUI();
    }

    public int GetSellPrice(InventoryItem item)
    {
        float percent = (currentVendor != null) ? currentVendor.sellPercent : 0.8f;
        return Mathf.Max(0, Mathf.RoundToInt(item.priceInCents * percent));
    }

    public int GetDisplayedPrice(InventoryItem item)
    {
        return (mode == ShopMode.Buy) ? item.priceInCents : GetSellPrice(item);
    }

    private string FormatCents(int cents)
    {
        float dollars = cents / 100f;
        return "$" + dollars.ToString("0.00");
    }

    private bool PlayerHasItem(InventoryItem item)
    {
        if (playerInventory == null || playerInventory.myInventory == null) return false;

        for (int i = 0; i < playerInventory.myInventory.Count; i++)
        {
            var invItem = playerInventory.myInventory[i];
            if (invItem == null) continue;

            if (invItem == item || invItem.itemName == item.itemName)
                return invItem.numberHeld > 0;
        }
        return false;
    }

    private void AddItemToPlayerInventory(InventoryItem item, int amount)
    {
        if (playerInventory == null) return;

        for (int i = 0; i < playerInventory.myInventory.Count; i++)
        {
            InventoryItem invItem = playerInventory.myInventory[i];
            if (invItem == null) continue;

            if (invItem == item || invItem.itemName == item.itemName)
            {
                invItem.numberHeld += amount;
                return;
            }
        }

        InventoryItem runtimeCopy = Instantiate(item);
        runtimeCopy.numberHeld = amount;
        playerInventory.myInventory.Add(runtimeCopy);
    }
}
