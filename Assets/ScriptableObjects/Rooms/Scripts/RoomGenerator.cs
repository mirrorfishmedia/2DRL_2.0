using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public RoomTemplate testRoom;
	public int roomSize = 10;
	public RoomTemplate[] roomTemplates;
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
                int x = (int)spawnPos.x;
                int y = (int)spawnPos.y;
                switch (selectedChar) 
				{
				case '1':
					boardGenerator.WriteToBoardGrid (MapCell.CellType.Wall, x, y);
					break;
				case '0':
                    boardGenerator.WriteToBoardGrid (MapCell.CellType.BlackFloor, x,y);
					break;
				case '3':
					boardGenerator.WriteToBoardGrid (MapCell.CellType.Exit, x, y);
					break;
				case '4':
                        MapCell.CellType type = WriteRandomTileToGrid(roomTemplate.enemyTiles);                  
                            boardGenerator.WriteToBoardGrid (type, x, y);
                    enemyController.AddEnemy(type, new Vector2(x, y));

                        break;
				case '5':
					boardGenerator.WriteToBoardGrid (WriteRandomTileToGrid(roomTemplate.pickUpTiles), x, y);
					break;
				case '6':
					boardGenerator.WriteToBoardGrid (WriteRandomTileToGrid(roomTemplate.obstacleTiles), x, y);
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

}