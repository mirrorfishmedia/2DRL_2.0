using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public RoomTemplate testRoom;
	public int roomSize = 10;
	public RoomTemplate[] roomTemplates;


	public Interaction exitInteraction;
	public Interaction treasureInteraction;
	public Interaction foodInteraction;

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
		}
	}

	public void Build(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber)
	{
		StringToSquare(roomOrigin, roomTemplate, chainNumber, true);
	}

	public void StringToSquare(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath)
	{

		char[,] processedCharData = AsciiToGrid(roomTemplate.roomData);

		if (isOnPath) 
		{
			GameObject roomHolder = new GameObject ("Path Room " + chainNumber);
			roomHolder.transform.position = roomOrigin;
		}


		for (int i = 0; i < roomSize; i++) 
		{
			for (int j = 0; j < roomSize; j++) {
				Vector2 spawnPos = new Vector2 (i, j) + roomOrigin;


				char selectedChar = processedCharData [j, i];

				switch (selectedChar) 
				{
				case '1':
					WriteToBoardGrid (Tile.TileType.Wall, (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '0':
					WriteToBoardGrid (Tile.TileType.BlackFloor, (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '3':
					WriteToBoardGrid (Tile.TileType.Exit, (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '4':
					WriteToBoardGrid (WriteRandomTileToGrid(roomTemplate.enemyTiles), (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '5':
					WriteToBoardGrid (WriteRandomTileToGrid(roomTemplate.pickUpTiles), (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '6':
					WriteToBoardGrid (WriteRandomTileToGrid(roomTemplate.obstacleTiles), (int)spawnPos.x, (int)spawnPos.y);
					break;
				default:
					break;
				}
			}
		}

	}

	private Tile.TileType WriteRandomTileToGrid(Tile.TileType[] possibleTypes)
	{
		Tile.TileType selectedType = possibleTypes [Random.Range (0, possibleTypes.Length)];
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

	void WriteToBoardGrid(Tile.TileType value, int gridX, int gridY)
	{
		boardGenerator.tileData [gridX, gridY].tileType = value;
		boardGenerator.tileData [gridX, gridY].interaction = AssignInteraction (value);
		Debug.Log ("write to board grid, interaction: " + boardGenerator.tileData [gridX, gridY].interaction);
	}

	Interaction AssignInteraction(Tile.TileType value)
	{
		switch (value) 
		{
		case Tile.TileType.Exit:
			return exitInteraction;
			Debug.Log ("assigning interaction to exit");
			break;
		case Tile.TileType.Coin:
			return treasureInteraction;
			Debug.Log ("assigning interaction to exit");
			break;
		case Tile.TileType.Mushroom:
			return foodInteraction;
			Debug.Log ("assigning interaction to exit");
			break;

		default:
			return null;
			break;
		}
	}


}