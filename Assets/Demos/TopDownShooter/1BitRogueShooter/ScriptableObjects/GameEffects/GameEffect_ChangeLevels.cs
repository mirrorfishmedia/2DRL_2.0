using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/ChangeLevels")]

public class GameEffect_ChangeLevels : GameEffect
{
    public override void TriggerEffect(GameObject triggeringObject, GameObject triggeredObject)
    {
        if (triggeringObject.GetComponent<PlayerIdentifier>() != null)
        {
            GameMan.gm.ChangeLevels();
            
        }
    }
}
