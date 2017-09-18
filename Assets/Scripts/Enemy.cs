using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public Vector2 enemyPos;
    public MapCell.CellType cellType;
    public int maxHp = 3;
    public int currentHp;

    private Transform enemyTransform;


    public void SetupEnemy(Vector2 position, Transform newTransform)
    {
        currentHp = maxHp;
        enemyPos = position;
        enemyTransform = newTransform;
    }

    public void ModifyHp(int hpChange)
    {
        currentHp += hpChange;
        Debug.Log("currentHp " + currentHp);
    }

    public void MoveToPlayer(Vector2 playerPosition, BoardGenerator boardGenerator)
    {
        int xDir = 0;
        int yDir = 0;

        if (playerPosition.x > enemyTransform.position.x)
        {
            xDir = 1;
            
        }
        else if (playerPosition.x == enemyTransform.position.x)
        {
            xDir = 0;
        }
        else
        {
            xDir = -1;
        }


        if (playerPosition.y > enemyTransform.position.y)
        {
            yDir = 1;
        }
        else if (playerPosition.y == enemyTransform.position.y)
        {
            yDir = 0;
        }
        else
        {
            yDir = -1;
        }

        if (yDir != 0)
        {
            xDir = 0;
        }
        else if (xDir != 0)
        {
            yDir = 0;
        }
        boardGenerator.TryMove(xDir, yDir, MapCell.CellType.Enemy1, enemyTransform);
        
    }

}
