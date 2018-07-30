using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour {

    public Image[] itemImages;

    private SimpleInventory simpleInventory;

    public void DisplayItems(Item[] items)
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (items[i] != null)
            {
                itemImages[i].sprite = items[i].itemIconSprite;
            }
            else
            {
                itemImages[i].sprite = null;
            }
        }
    }
}
