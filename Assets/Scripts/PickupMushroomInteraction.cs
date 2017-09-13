using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/PickupMushroomInteraction")]
public class PickupMushroomInteraction : Interaction 
{
	public override void RespondToInteraction (MapCell targetTile)
	{
		GameManager.instance.ModifyFood (1);
		targetTile.cellType = MapCell.CellType.BlackFloor;
		Debug.Log ("food interaction triggered");
	}
}

