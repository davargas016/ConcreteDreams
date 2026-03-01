using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class CoinTextManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public TextMeshProUGUI wallet;
    decimal totalDollars;

    public void OnEnable()
    {
        totalDollars = playerInventory.money / 100m;
        wallet.text = totalDollars.ToString("F2");
    }

    public void UpdateMoneyCount()
    {
        totalDollars = playerInventory.money / 100m;
        wallet.text = totalDollars.ToString("F2");
    }
    
}
