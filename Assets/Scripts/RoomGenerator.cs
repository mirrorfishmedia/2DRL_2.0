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
    private EnemyController enemyController;

	void Awake()
	{
		boardGenerator = GetComponent<BoardGenerator> ();
        enemyController = GetComponent<EnemyController>();
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
					WriteToBoardGrid (MapCell.CellType.Wall, (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '0':
					WriteToBoardGrid (MapCell.CellType.BlackFloor, (int)spawnPos.x, (int)spawnPos.y);
					break;
				case '3':
					WriteToBoardGrid (MapCell.CellType.Exit, (int)spawnPos.x, (int)spawnPos.y);
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

	void WriteToBoardGrid(MapCell.CellType value, int x, int y)
	{
        switch (value)
        {
            case MapCell.CellType.BlackFloor:
                break;
            case MapCell.CellType.GrassFloor:
                break;
            case MapCell.CellType.Wall:
                break;
            case MapCell.CellType.Player:
                break;
            case MapCell.CellType.Coin:
                break;
            case MapCell.CellType.Mushroom:
                break;
            case MapCell.CellType.Enemy1:
                enemyController.AddEnemy(MapCell.CellType.Enemy1, new Vector2(x, y));

                break;
            case MapCell.CellType.Enemy2:
                enemyController.AddEnemy(MapCell.CellType.Enemy2, new Vector2(x, y));

                break;
            case MapCell.CellType.Obstacle:
                break;
            case MapCell.CellType.Exit:
                boardGenerator.exitLocations.Add(new Vector2 (x,y));
                break;
            default:
                break;
        }
        boardGenerator.tileData [x, y].cellType = value;
		boardGenerator.tileData [x, y].interaction = AssignInteraction (value);

    }

	Interaction AssignInteraction(MapCell.CellType value)
	{
		switch (value) 
		{
		case MapCell.CellType.Exit:
			return exitInteraction;
			break;
		case MapCell.CellType.Coin:
			return treasureInteraction;
			break;
		case MapCell.CellType.Mushroom:
			return foodInteraction;
			break;

		default:
			return null;
			break;
		}
	}


}