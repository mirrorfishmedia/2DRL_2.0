using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour {


    private Item[] items = new Item[4];
    private InventoryDisplay inventoryDisplay;

    private void Awake()
    {
        inventoryDisplay = GetComponentInChildren<InventoryDisplay>();
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                UpdateInventoryDisplay();

                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            UseItem(0);

        }
    }

    void UseItem(int itemNumber)
    {
        items[0].gameEffect.TriggerEffect(this.gameObject);
        items[0] = null;
        UpdateInventoryDisplay();
    }

    void UpdateInventoryDisplay()
    {
        inventoryDisplay.DisplayItems(items);
    }
}
