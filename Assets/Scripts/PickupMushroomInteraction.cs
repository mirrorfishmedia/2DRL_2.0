using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/PickupMushroomInteraction")]
public class PickupMushroomInteraction : Interaction 
{
	public override void RespondToInteraction (Tile targetTile)
	{
		GameManager.instance.ModifyFood (1);
		targetTile.tileType = Tile.TileType.BlackFloor;
		Debug.Log ("food interaction triggered");
	}
}

