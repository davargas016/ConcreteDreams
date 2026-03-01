using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Money : Item
{

    public PlayerInventory playerInventory;
    public MoneyDenomination denomination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSignal.Raise();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            AddValue(denomination);
            Destroy(this.gameObject);
        }
    }
    
    public void AddValue(MoneyDenomination denomination)
    {
        playerInventory.money += denomination.value;
        itemSignal.Raise();
    }
}
