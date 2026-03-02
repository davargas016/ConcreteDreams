using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour
{
    public enum VendorType
    {
        General,
        Fish
    }

    [Header("Vendor Type")]
    public VendorType vendorType = VendorType.General;

    [Header("Items this vendor sells")]
    public List<InventoryItem> itemsForSale = new List<InventoryItem>();

    [Header("Sell payout percent (of buy price)")]
    [Range(0f, 1f)] public float sellPercent = 0.8f;
}
