using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageEffect : ScriptableObject
{
    public abstract void HandleDamage(GameObject damagedObject);
}
