using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemCategory
{
    General,
    Fish
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemImage;

    [Header("Inventory")]
    public int numberHeld;
    public bool usable;   // Can it be used?
    public bool unique;   // Consumed only once?
    public int priceInCents;
    
    public UnityEvent thisEvent;

    [Header("Health")]
    public float healthRestore;

    [Header("Needs Effects (optional)")]
    public float hungerAmount;
    public float thirstAmount;

    [Header("Shop")]
    public ItemCategory category = ItemCategory.General;
    public bool sellable = true;
    
    public void Use()
    {
        thisEvent.Invoke();
    }

    public void DecreaseAmount(int amountToDecrease)
    {
        numberHeld -= amountToDecrease;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }
}
