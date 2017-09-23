using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "2DRL/MapCellObject")]
public class MapCellObject : ScriptableObject
{
    public Tile tile;
    public Interaction interaction;

/*
 * To do: 
 * Create an editor screen based on editing tilemap to make rooms
 * 
 * Move grass onto a separate background layer
 * 
 * Make floor null in main interaction map
 * 
 * Decouple stuff so that moving sprites are on own layer and can be lerped and animated.
 */


}
