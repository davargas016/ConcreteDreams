using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
        }
    }
}
