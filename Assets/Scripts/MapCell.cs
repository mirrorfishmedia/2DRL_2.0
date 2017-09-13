using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapCell  
{
	public enum CellType {BlackFloor, GrassFloor, Wall, Player, Coin, Mushroom, Enemy1, Enemy2, Obstacle, Exit};
	public CellType cellType;
	public Interaction interaction;
}
