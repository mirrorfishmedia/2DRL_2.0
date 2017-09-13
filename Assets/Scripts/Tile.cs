using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile  
{
	public enum TileType {BlackFloor, GrassFloor, Wall, Player, Coin, Mushroom, Enemy1, Enemy2, Obstacle, Exit};
	public TileType tileType;
	public Interaction interaction;
}
