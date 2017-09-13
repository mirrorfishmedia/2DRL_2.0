using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/RoomTemplate")]
public class RoomTemplate : ScriptableObject 
{
	[TextArea(10,20)]
	public string roomData;
	public RoomList northList;
	public RoomList southList;
	public RoomList eastList;
	public RoomList westList;

	public Tile.TileType[] enemyTiles;
	public Tile.TileType[] pickUpTiles;
	public Tile.TileType[] obstacleTiles;


	public RoomAndDirection ChooseNextRoom(BoardGenerator generator, Vector2 currentLocation, List<Vector2> usedSpaces)
	{
		int direction = Random.Range (0, 4);

		List<RoomAndDirection> results = new List<RoomAndDirection> ();

		if (direction == 0 && northList != null) 
		{
			Vector2 northDir = new Vector2 (0, generator.roomSize);

			if (SpaceValid(currentLocation + northDir, usedSpaces, generator))
			{
				RoomAndDirection result = new RoomAndDirection ();
				result.selectedDirection = northDir;
				result.selectedRoom = northList.rooms [Random.Range (0, northList.rooms.Length)];
				results.Add (result);
			}

		}

		if (direction == 1 && southList != null) 
		{
			Vector2 southDir = new Vector2 (0, -generator.roomSize);

			if (SpaceValid(currentLocation + southDir, usedSpaces, generator))
			{
				RoomAndDirection result = new RoomAndDirection ();
				result.selectedDirection = southDir;
				result.selectedRoom = southList.rooms [Random.Range (0, southList.rooms.Length)];
				results.Add (result);
			}
		}



		if (direction == 2 && eastList != null) 
		{
			Vector2 eastDir = new Vector2 (generator.roomSize,0);

			if (SpaceValid(currentLocation + eastDir, usedSpaces, generator))
			{
				RoomAndDirection result = new RoomAndDirection ();
				result.selectedDirection = eastDir;
				result.selectedRoom = eastList.rooms [Random.Range (0, eastList.rooms.Length)];
				results.Add (result);
			}
		}

		if (direction == 3 && westList != null) 
		{
			Vector2 westDir = new Vector2 (-generator.roomSize,0 );

			if (SpaceValid(currentLocation + westDir, usedSpaces, generator))
			{
				RoomAndDirection result = new RoomAndDirection ();
				result.selectedDirection = westDir;
				result.selectedRoom = westList.rooms [Random.Range (0, westList.rooms.Length)];
				results.Add (result);
			}
		}

		if (results.Count == 0) 
		{
			return null;
		}

		RoomAndDirection selectedResult = results [Random.Range (0, results.Count)];


		return selectedResult;

	}

	bool SpaceValid(Vector2 spaceToTest, List<Vector2> usedSpaces, BoardGenerator generator)
	{
		if (usedSpaces.Contains(spaceToTest))
		{
			Debug.Log ("space filled, cant build room");
			return false;

		}
		else 
		{
			if (spaceToTest.x < generator.boardHorizontalSize && spaceToTest.y < generator.boardVerticalSize && spaceToTest.x > 0 && spaceToTest.y > 0) {

				Debug.Log ("space not filled, in board");
				return true;

			} else {
				Debug.Log ("out of board");
				return false;
			}


		}
	}


}
