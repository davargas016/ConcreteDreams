using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public List<InventoryItem> myInventory = new List<InventoryItem>();
    public int money; // cents or whatever you store

    public event Action Changed;

    public int GetItemCount(string itemName)
    {
        int total = 0;
        foreach (var item in myInventory)
        {
            if (item != null && item.itemName == itemName)
                total += item.numberHeld;
        }
        return total;
    }

    public void AddInventoryItem(InventoryItem item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        if (!myInventory.Contains(item))
            myInventory.Add(item);

        item.numberHeld += amount;

        Changed?.Invoke();
    }

    public bool RemoveItemByName(string itemName, int amount)
    {
        if (string.IsNullOrEmpty(itemName) || amount <= 0) return false;

        for (int i = myInventory.Count - 1; i >= 0 && amount > 0; i--)
        {
            var it = myInventory[i];
            if (it == null) continue;
            if (it.itemName != itemName) continue;

            int take = Mathf.Min(it.numberHeld, amount);
            it.numberHeld -= take;
            amount -= take;

            if (it.numberHeld <= 0)
                it.numberHeld = 0;
        }

        return amount == 0;
    }


    public void AddMoney(int amount)
    {
        money += amount;

        // notify quest system + UI
        if (QuestManager.I != null)
            QuestManager.I.OnMoneyChanged();
    }


    public bool SpendMoney(int amount)
    {
        if (amount <= 0) return true;
        if (money < amount) return false;

        money -= amount;
        Changed?.Invoke();
        return true;
    }

    // Use this if some legacy code still sets money directly
    public void SetMoney(int total)
    {
        money = total;
        Changed?.Invoke();
    }
}