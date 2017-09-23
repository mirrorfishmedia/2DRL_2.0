using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/AttackEnemyInteraction")]
public class AttackEnemyInteraction : Interaction
{
    public override void RespondToInteraction(MapCell targetTile)
    {
        Debug.Log("attacked Enemy, targetTile: " + targetTile);
        Vector2 enemyPos = new Vector2(targetTile.x, targetTile.y);
        Debug.Log("enemyPos " + enemyPos);
        Enemy enemy = GameManager.instance.enemyController.GetEnemyFromPosition(enemyPos);
        Debug.Log("found enemy with hp: " + enemy.currentHp);
        enemy.ModifyHp(-1);
    }
}
