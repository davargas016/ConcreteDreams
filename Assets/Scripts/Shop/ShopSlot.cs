using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    [Header("UI Stuff to Change")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI priceText;

    private InventoryItem item;
    private ShopManager manager;

    public void Setup(InventoryItem newItem, ShopManager newManager)
    {
        item = newItem;
        manager = newManager;

        if (item == null || manager == null) return;

        if (itemImage != null) itemImage.sprite = item.itemImage;

        if (priceText != null)
        {
            int cents = manager.GetDisplayedPrice(item);
            priceText.text = "$" + (cents / 100f).ToString("0.00");
        }
    }

    public void ClickedOn()
    {
        if (item == null || manager == null) return;
        manager.SetupDetailsFromSlot(item, itemImage != null ? itemImage.sprite : null);
    }
}
