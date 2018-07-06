using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public RoomTemplate testRoom;
	public int roomSize = 10;
	public RoomTemplate[] roomTemplates;
    public bool buildOnStart;

	private BoardGenerator boardGenerator;

	void Awake()
	{
		boardGenerator = GetComponent<BoardGenerator> ();
    }

	void Start()
	{
		if (buildOnStart) 
		{
			Build (new Vector2 (50, 50), testRoom, 0);
            //boardGenerator.DisplayTilemapInFrustum((Vector2)GameManager.instance.player.position);
        }
	}

	public void Build(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber)
	{
        //StringToSquare(roomOrigin, roomTemplate, chainNumber, true);
        ScriptableRoom(roomOrigin, roomTemplate, chainNumber, true);

    }

    public void ScriptableRoom(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath)
    {

        if (isOnPath)
        {
            GameObject roomHolder = new GameObject("Path Room " + chainNumber);
            roomHolder.transform.position = roomOrigin;
        }
        int charIndex = 0;
        
        for (int i = 0; i < roomSize; i++)
        {
            for (int j = 0; j < roomSize; j++)
            {
                char selectedChar = roomTemplate.roomChars[charIndex];
                if (selectedChar != '\0')
                {
                    Vector2 spawnPos = new Vector2(i, j) + roomOrigin;

                    int x = (int)spawnPos.x;
                    int y = (int)spawnPos.y;

                    boardGenerator.boardGridAsCharacters[x, y] = selectedChar;
                }
                 
                charIndex++;

            }
        }
    }

	private MapCell.CellType WriteRandomTileToGrid(MapCell.CellType[] possibleTypes)
	{
		MapCell.CellType selectedType = possibleTypes [Random.Range (0, possibleTypes.Length)];
		return selectedType;
	}

	char[,] AsciiToGrid(string toClear)
	{

		toClear = toClear.Replace ("\n", "").Replace ("\r", "");
		string reversedData = "";
		for (int i = toClear.Length - 1; i >= 0; i--)
		{
			reversedData += toClear[i];
		}

		char[,] roomChars = new char[roomSize,roomSize];

		int charIndex = 0;
		for (int x = 0; x < roomSize; x++ )
		{
			for (int y = 0; y <roomSize; y++) 
			{
				roomChars [x, y] = reversedData [charIndex];
				charIndex++;
			}
		}

		char[,] rotatedRoomChars = new char[roomSize,roomSize];

		for (int i=roomSize-1; i >=0; --i)
		{
			for (int j=0; j < roomSize; ++j)
			{
				rotatedRoomChars[j,(roomSize-1)-i] = roomChars[j,i];
			}
		}

		return rotatedRoomChars;
	}

}