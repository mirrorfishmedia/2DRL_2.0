using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapCell  
{
	public enum CellType {BlackFloor, GrassFloor, Wall, Player, Coin, Mushroom, Enemy1, Enemy2, Obstacle, Exit};
    public MapCellObject mapCellObject;
	public CellType cellType;
	public Interaction interaction;
    public int x;
    public int y;

}
