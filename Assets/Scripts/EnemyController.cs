using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(MapCell.CellType type, Vector2 position)
    {
        Enemy enemy = new Enemy();

        Transform enemyTransform = new GameObject("Enemy").transform;
        enemyTransform.position = position;
        enemy.cellType = type;
        enemy.SetupEnemy(position, enemyTransform);
        enemies.Add(enemy);
    }

    public void UpdateEnemies(BoardGenerator boardGenerator)
    {
        for (int i = enemies.Count-1; i > 0; i--)
        {
            if (CheckIfDead(enemies[i]))
            {
                boardGenerator.WriteToBoardGrid(MapCell.CellType.BlackFloor, enemies[i].x, enemies[i].y);
                enemies.RemoveAt(i);
            }

            enemies[i].MoveToPlayer((Vector2)GameManager.instance.player.position, boardGenerator);
        }
    }


    private bool CheckIfDead(Enemy enemy)
    {
        if (enemy.currentHp <= 0)
        {
            return true;
        }
        return false;
    }

    public Enemy GetEnemyFromPosition(Vector2 enemyPosition)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if ((Vector2)enemies[i].enemyTransform.position == enemyPosition)
            {
                return enemies[i];
            }
           
        }

        return null;
    }
}
