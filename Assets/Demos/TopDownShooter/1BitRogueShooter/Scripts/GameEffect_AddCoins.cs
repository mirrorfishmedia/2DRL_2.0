using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/AddCoins")]

public class GameEffect_AddCoins : GameEffect
{
    public int coinValue = 1;

    public override void TriggerEffect(GameObject triggeringObject, GameObject triggeredObject)
    {
        if (triggeringObject.GetComponent<PlayerIdentifier>())
        {
            triggeredObject.SetActive(false);
            GameMan.gm.CollectGold(coinValue);
            
        }

    }
}
