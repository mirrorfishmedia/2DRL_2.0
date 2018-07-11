using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoardGenerator/Templates/RoomTemplate")]
public class RoomTemplate : ScriptableObject 
{

    public int roomSizeX = 10;
    public int roomSizeY = 10;
    public BoardLibrary library;

    public char[] roomChars = new char[100];

	public RoomAndDirection ChooseNextRoom(BoardGenerator boardGenerator, Vector2 currentLocation, List<Vector2> usedSpaces)
	{       
        for (int z = 0; z < 400; z++)
        {

            List<RoomAndDirection> results = new List<RoomAndDirection>();
            results.Clear();
            RoomAndDirection result = new RoomAndDirection();

            int direction = Random.Range(0, 4);
            if (direction == 0 && boardGenerator.northList != null)
            {
                result.selectedRoom = boardGenerator.northList.rooms[Random.Range(0, boardGenerator.northList.rooms.Length)];
                Vector2 northDir = new Vector2(0, result.selectedRoom.roomSizeY);

                if (SpaceValid(currentLocation + northDir, usedSpaces, boardGenerator))
                {
                    result.selectedDirection = northDir;
                    results.Add(result);
                }
            }

            if (direction == 1 && boardGenerator.southList != null)
            {
                result.selectedRoom = boardGenerator.southList.rooms[Random.Range(0, boardGenerator.southList.rooms.Length)];

                Vector2 southDir = new Vector2(0, -result.selectedRoom.roomSizeY);

                if (SpaceValid(currentLocation + southDir, usedSpaces, boardGenerator))
                {
                    result.selectedDirection = southDir;
                    results.Add(result);
                }
            }



            if (direction == 2 && boardGenerator.eastList != null)
            {
                result.selectedRoom = boardGenerator.eastList.rooms[Random.Range(0, boardGenerator.eastList.rooms.Length)];

                Vector2 eastDir = new Vector2(result.selectedRoom.roomSizeX, 0);

                if (SpaceValid(currentLocation + eastDir, usedSpaces, boardGenerator))
                {
                    result.selectedDirection = eastDir;
                    results.Add(result);
                }
            }

            if (direction == 3 && boardGenerator.westList != null)
            {
                result.selectedRoom = boardGenerator.westList.rooms[Random.Range(0, boardGenerator.westList.rooms.Length)];

                Vector2 westDir = new Vector2(-result.selectedRoom.roomSizeX, 0);

                if (SpaceValid(currentLocation + westDir, usedSpaces, boardGenerator))
                {
                    result.selectedDirection = westDir;
                    results.Add(result);
                }
            }

            if (results.Count != 0)
            {
                RoomAndDirection selectedResult = results[Random.Range(0, results.Count)];
                return selectedResult;
            }
        }

        return null;

	}

	bool SpaceValid(Vector2 spaceToTest, List<Vector2> usedSpaces, BoardGenerator generator)
	{
		if (usedSpaces.Contains(spaceToTest))
		{
			//Debug.Log ("space filled, cant build room");
			return false;
		}
		else 
		{
			if (spaceToTest.x < generator.boardHorizontalSize && spaceToTest.y < generator.boardVerticalSize && spaceToTest.x > 0 && spaceToTest.y > 0) {

				//Debug.Log ("space not filled, in board");
				return true;

			} else {
				//Debug.Log ("out of board");
				return false;
			}
		}
	}


}
