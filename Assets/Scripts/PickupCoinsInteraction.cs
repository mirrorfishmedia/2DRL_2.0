using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/PickupCoinsInteraction")]
public class PickupCoinsInteraction : Interaction 
{
	public override void RespondToInteraction (MapCell targetTile)
	{
		GameManager.instance.ModifyCoins (1);
		targetTile.cellType = MapCell.CellType.BlackFloor;
		Debug.Log ("coin interaction triggered");
	}
}
