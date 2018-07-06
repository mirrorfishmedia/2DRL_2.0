using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2DRL/RoomTemplate")]
public class RoomTemplate : ScriptableObject 
{

    public int roomSizeX = 10;
    public int roomSizeY = 10;
    public BoardLibrary library;
	public RoomList northList;
	public RoomList southList;
	public RoomList eastList;
	public RoomList westList;




    public char[] roomChars = new char[100];

    public Vector2 FindUnblockedRandomSpace(Vector2 currentLocation, List<Vector2> usedSpaces)
    {
        List<Vector2> possibleNextRoomLocations = new List<Vector2>();
        Vector2 dirToNextLocation = Vector2.zero;

        possibleNextRoomLocations.Add(currentLocation + new Vector2(1, 0));
        possibleNextRoomLocations.Add(currentLocation + new Vector2(0, 1));
        possibleNextRoomLocations.Add(currentLocation + new Vector2(-1, 0));
        possibleNextRoomLocations.Add(currentLocation + new Vector2(0, -1));

        for (int i = 0; i < possibleNextRoomLocations.Count; i++)
        {
            for (int j = 0; j < usedSpaces.Count; j++)
            {
                if (possibleNextRoomLocations.Contains(usedSpaces[j]))
                {
                    possibleNextRoomLocations.Remove(usedSpaces[j]);
                }
            }
        }
        
        Vector2 selectedLocation = possibleNextRoomLocations[Random.Range(0, possibleNextRoomLocations.Count)];
       
        return selectedLocation;
    }

	public RoomAndDirection ChooseNextRoom(BoardGenerator generator, Vector2 currentLocation, List<Vector2> usedSpaces)
	{       
		

        for (int z = 0; z < 400; z++)
        {

            List<RoomAndDirection> results = new List<RoomAndDirection>();
            results.Clear();
            RoomAndDirection result = new RoomAndDirection();

            int direction = Random.Range(0, 4);
            if (direction == 0 && northList != null)
            {
                result.selectedRoom = northList.rooms[Random.Range(0, northList.rooms.Length)];
                Vector2 northDir = new Vector2(0, result.selectedRoom.roomSizeY);

                if (SpaceValid(currentLocation + northDir, usedSpaces, generator))
                {
                    result.selectedDirection = northDir;
                    results.Add(result);
                }
            }

            if (direction == 1 && southList != null)
            {
                Vector2 southDir = new Vector2(0, -generator.roomSize);

                if (SpaceValid(currentLocation + southDir, usedSpaces, generator))
                {
                    result.selectedDirection = southDir;
                    result.selectedRoom = southList.rooms[Random.Range(0, southList.rooms.Length)];
                    results.Add(result);
                }
            }



            if (direction == 2 && eastList != null)
            {
                Vector2 eastDir = new Vector2(generator.roomSize, 0);

                if (SpaceValid(currentLocation + eastDir, usedSpaces, generator))
                {
                    result.selectedDirection = eastDir;
                    result.selectedRoom = eastList.rooms[Random.Range(0, eastList.rooms.Length)];
                    results.Add(result);
                }
            }

            if (direction == 3 && westList != null)
            {
                Vector2 westDir = new Vector2(-generator.roomSize, 0);

                if (SpaceValid(currentLocation + westDir, usedSpaces, generator))
                {
                    result.selectedDirection = westDir;
                    result.selectedRoom = westList.rooms[Random.Range(0, westList.rooms.Length)];
                    results.Add(result);
                }
            }


            if (results.Count != 0)
            {
                RoomAndDirection selectedResult = results[Random.Range(0, results.Count)];
                return selectedResult;
            }
        }

        

        //Debug.LogError("room generation failed and returned null!");
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
