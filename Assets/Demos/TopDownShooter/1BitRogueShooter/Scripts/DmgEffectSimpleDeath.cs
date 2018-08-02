using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TDR_Demo/Damage Effect Simple Death")]
public class DmgEffectSimpleDeath : DamageEffect
{
    public override void HandleDamage(GameObject damagedObject)
    {
        //damagedObject.SetActive(false);
    }
}
