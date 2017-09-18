using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/AttackEnemyInteraction")]
public class AttackEnemyInteraction : Interaction
{
    public override void RespondToInteraction(MapCell targetTile)
    {
        Debug.Log("attacked Enemy, targetTile: " + targetTile);
    }
}
