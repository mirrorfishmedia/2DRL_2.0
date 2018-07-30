using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTeleportPotion : CollectibleAction {

    public override void OnCollection(GameObject collectingObject)
    {
        CollectTeleport();
    }

    void CollectTeleport()
    {

    }
}
