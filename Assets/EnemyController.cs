using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();

    public void AddEnemy(MapCell.CellType type)
    {
        Enemy enemy = new Enemy();
        enemy.cellType = type;
        enemies.Add(enemy);
    }
}
