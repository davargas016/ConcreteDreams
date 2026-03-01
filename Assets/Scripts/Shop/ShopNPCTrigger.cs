using UnityEngine;

public class ShopNPCTrigger : MonoBehaviour
{
    private ShopToggle playerShopToggle;
    private Vendor vendor;
    public Signal contextOn;
    public Signal contextOff;

    private void Awake()
    {
        vendor = GetComponent<Vendor>();
        if (vendor == null)
            Debug.LogWarning($"{name}: ShopNPCTrigger has no Vendor component.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        contextOn.Raise();

        playerShopToggle = other.GetComponent<ShopToggle>();
        if (playerShopToggle != null)
        {
            playerShopToggle.canOpenShop = true;
            playerShopToggle.currentVendor = vendor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        contextOff.Raise();

        if (playerShopToggle != null)
        {
            playerShopToggle.canOpenShop = false;
            playerShopToggle.CloseShop();
            playerShopToggle.currentVendor = null;
        }
    }
}
