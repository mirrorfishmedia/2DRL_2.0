﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInventory : MonoBehaviour {


    public Item[] items = new Item[4];
    private InventoryDisplay inventoryDisplay;

    private void Awake()
    {
        inventoryDisplay = GetComponentInChildren<InventoryDisplay>();
        inventoryDisplay.DisplayItems(items); 
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
        items[itemNumber].gameEffect.TriggerEffect(this.gameObject, this.gameObject);
        items[itemNumber] = null;
        UpdateInventoryDisplay();
    }

    void UpdateInventoryDisplay()
    {
        inventoryDisplay.DisplayItems(items);
    }
}
