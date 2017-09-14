using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public Vector2 enemyPos;
    public bool seekingPlayer;
    public MapCell.CellType cellType;

    private Transform enemyTransform;
  
    public void SetupEnemy(Vector2 position, Transform newTransform)
    {
        enemyPos = position;
        enemyTransform = newTransform;
    }

    public void MoveToPlayer(Vector2 playerPosition, BoardGenerator boardGenerator)
    {
        Debug.Log("Move to player: " + playerPosition);
        int xDir = 0;
        int yDir = 0;


        if (Mathf.Abs(playerPosition.y - enemyPos.y) < float.Epsilon)
        {
            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = playerPosition.y > enemyPos.y ? 1 : -1;
        }
        //If the difference in positions is not approximately zero (Epsilon) do the following:
        else
        {
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = playerPosition.x > enemyPos.x ? 1 : -1;
        }

        //Vector2 updatedPos = enemyPos + new Vector2(xDir, yDir);
        //enemyPos = updatedPos;
        //boardGenerator.TrackMovingUnit(updatedPos, cellType);
        boardGenerator.TryMove(xDir, yDir, MapCell.CellType.Enemy1, enemyTransform);
        
    }

}
