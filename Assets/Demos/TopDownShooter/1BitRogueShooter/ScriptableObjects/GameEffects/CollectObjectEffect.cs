using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/CollectObject")]
public class CollectObjectEffect : GameEffect {

    public override void TriggerEffect(GameObject triggeringObject, GameObject triggeredObject)
    {
        SimpleInventory inventory = triggeringObject.gameObject.GetComponent<SimpleInventory>();
        if (inventory != null)
        {
            Item collectibleItem = triggeredObject.GetComponent<EffectOnCollision>().item;
            if (collectibleItem != null)
            {
                if (inventory.AddItem(collectibleItem))
                {
                    triggeredObject.SetActive(false);
                }
            }
        }
    }
}
