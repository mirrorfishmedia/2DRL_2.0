using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour {

    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SimpleInventory inventory = collision.gameObject.GetComponent<SimpleInventory>();
        if (inventory != null)
        {
            if (inventory.AddItem(item))
            {
                gameObject.SetActive(false);
            }
        }

    }
}
