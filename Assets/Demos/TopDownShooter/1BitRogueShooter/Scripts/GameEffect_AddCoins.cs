using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/AddCoins")]

public class GameEffect_AddCoins : GameEffect
{
    public int coinValue = 1;

    public override bool TriggerEffect(GameObject triggeringObject)
    {
        if (triggeringObject.GetComponent<PlayerIdentifier>())
        {
            GameMan.gm.CollectGold(coinValue);
            return true;

        }

        return false;
    }
}
