using UnityEngine;

public class ShopToggle : MonoBehaviour
{
    public GameObject shopScreen;
    public static bool shopOpen = false;

    [HideInInspector] public bool canOpenShop = false;
    [HideInInspector] public Vendor currentVendor;

    [SerializeField] private ShopManager shopManager;

    void Update()
    {
        if (canOpenShop && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        if (shopScreen == null)
        {
            Debug.LogError("ShopToggle: shopScreen is not assigned!");
            return;
        }

        bool newState = !shopScreen.activeSelf;
        shopScreen.SetActive(newState);
        shopOpen = newState;

        if (newState && shopManager != null)
        {
            shopManager.OpenVendor(currentVendor);
        }
    }

    public void CloseShop()
    {
        if (shopScreen == null) return;

        shopScreen.SetActive(false);
        shopOpen = false;
    }
}
