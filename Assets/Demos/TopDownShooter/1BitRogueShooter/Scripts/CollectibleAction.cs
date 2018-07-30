using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CollectibleAction : ScriptableObject
{
    public abstract void OnCollection(GameObject collectingObject);	
}
