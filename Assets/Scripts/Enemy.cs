using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public bool seekingPlayer;
    public MapCell.CellType cellType;

    public void MoveToPlayer(Vector2 playerPosition, Vector2 currentPosition, BoardGenerator boardGenerator)
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(playerPosition.x - currentPosition.x) < float.Epsilon)
        {
            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = playerPosition.y > currentPosition.y ? 1 : -1;
        }
        //If the difference in positions is not approximately zero (Epsilon) do the following:
        else
        {
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = playerPosition.x > currentPosition.x ? 1 : -1;
        }

       Vector2 updatedPos = currentPosition + new Vector2(xDir, yDir);

        boardGenerator.TrackMovingUnit(updatedPos, cellType);

        
    }

}
