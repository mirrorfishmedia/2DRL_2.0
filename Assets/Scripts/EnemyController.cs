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
}
