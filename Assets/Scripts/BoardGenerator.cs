using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardGenerator : MonoBehaviour {

    public bool buildOnStart;

    public int boardHorizontalSize = 8;
	public int boardVerticalSize = 8;
    public Tilemap tilemap;

    public Generator[] generators;
    public char emptySpaceChar = '0';

    public List<Vector2> exitLocations;

	private bool roomChainHitEdge;
	public List<Vector2> usedSpaces = new List<Vector2>();
	public Vector2 currentLocation;
    public int roomsOnPathCreated;
	public RoomTemplate currentRoom;

    public char[,] boardGridAsCharacters;
    private BoardInstantiator boardInstantiator;


    void Awake()
	{
        boardInstantiator = GetComponent<BoardInstantiator>();
    }

    // Use this for initialization
    void Start () 
	{
        if (buildOnStart)
        {
            BuildLevel();
        }
    }

    void BuildLevel()
    {
        boardInstantiator.Initialize();
        SetupEmptyGrid();
        RunGenerators();
        InstantiateGeneratedLevelData();
    }

    void SetupEmptyGrid()
    {
        boardGridAsCharacters = new char[boardHorizontalSize, boardVerticalSize];
        for (int i = 0; i < boardHorizontalSize; i++)
        {
            for (int j = 0; j < boardVerticalSize; j++)
            {
                boardGridAsCharacters[i, j] = emptySpaceChar;
            }
        }
    }

    void RunGenerators()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].Generate(this);
        }
    }

    void InstantiateGeneratedLevelData()
    {
        for (int x = 0; x < boardHorizontalSize; x++)
        {
            for (int y = 0; y < boardVerticalSize; y++)
            {
                Vector2 spawnPos = new Vector2(x, y);
                boardInstantiator.InstantiateTile(spawnPos, boardGridAsCharacters[x, y]);
            }
        }
    }

    bool SpaceValid(Vector2 spaceToTest)
	{
		if (usedSpaces.Contains(spaceToTest))
		{
			Debug.Log ("space filled");
			return false;
		}
		else 
		{
			Debug.Log ("space empty");
			if (spaceToTest.x < boardHorizontalSize && spaceToTest.y < boardVerticalSize && spaceToTest.x > 0 && spaceToTest.y > 0) {
				Debug.Log ("space valid in board, roomsOnPathCreated: " + roomsOnPathCreated);
				return true;
			} else {
				Debug.Log ("reached board edge: " + roomsOnPathCreated);
				return false;
			}
		}
	}

    public GridPosition GetRandomGridPosition()
    {
        GridPosition randomPosition = new GridPosition(Random.Range(0, boardHorizontalSize), Random.Range(0, boardVerticalSize));
        return randomPosition;
    }

    public void DrawTemplate(int x, int y, RoomTemplate templateToSpawn, bool overWriteFilledCharacters)
    {
        int charIndex = 0;
        for (int i = 0; i < templateToSpawn.roomSizeX; i++)
        {
            for (int j = 0; j < templateToSpawn.roomSizeY; j++)
            {
                WriteToBoardGrid(x + i, y + j, templateToSpawn.roomChars[charIndex], overWriteFilledCharacters);
                charIndex++;
            }
        }
    }


    bool TestIfInGrid(int x, int y)
	{
		if (x < boardHorizontalSize && y < boardVerticalSize && x >= 0 && y >= 0) 
		{
			return true;
		} else 
		{
			return false;
		}
			
	}


    public void WriteToBoardGrid(int x, int y, char charIdToWrite, bool overwriteFilledSpaces)
    {
        if (TestIfInGrid(x, y))
        {
            if (overwriteFilledSpaces)
            {
                boardGridAsCharacters[x, y] = charIdToWrite;
            }
            else
            {
                if (boardGridAsCharacters[x, y] == emptySpaceChar)
                {
                    boardGridAsCharacters[x, y] = charIdToWrite;
                }
            }
        }
        
        
    }

    public void SetTileFromGrid(char charId, int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        boardInstantiator.InstantiateTile(pos, charId);
    }
    /*
	public void DisplayTilemapInFrustum(Vector2 playerPos)
	{
        tilemap.ClearAllTiles();
		int playerX = (int)playerPos.x;
		int playerY = (int)playerPos.y;

		int frustumStartX = playerX - (cameraFrustumX / 2);
		int frustumStartY = playerY - (cameraFrustumY / 2);

		for (int x = frustumStartX; x < frustumStartX + cameraFrustumX; x++) {
			for (int y = frustumStartY; y < frustumStartY + cameraFrustumY; y++) 
			{
				if (TestIfInGrid (x,y)) 
				{
					MapCell.CellType tileDataValue = tileData[x,y].cellType;
					switch (tileDataValue) 
					{
					case MapCell.CellType.BlackFloor:
                            SetTileFromGrid(blackFloorTile, x, y);
						break;
                    case MapCell.CellType.GrassFloor:
                        SetTileFromGrid(grassTile, x, y);
                        break;
                        case MapCell.CellType.Wall:
                            SetTileFromGrid(wall, x, y);
                        break;
					case MapCell.CellType.Exit:
                            SetTileFromGrid(exitTile, x, y);
						break;
					case MapCell.CellType.Coin:
                            SetTileFromGrid(coin, x, y);
                            break;
                        case MapCell.CellType.Mushroom:
                            SetTileFromGrid(mushroom, x, y);
                            break;
					case MapCell.CellType.Enemy1:
                            SetTileFromGrid(enemy1, x, y);
                            break;
					case MapCell.CellType.Enemy2:
                            SetTileFromGrid(enemy2, x, y);
                            break;

					}
				}

			}
		}
	}
    */

   

    /*
    void ChooseExit()
    {
        Vector2 exitLocation = exitLocations[exitLocations.Count-1];
        Debug.Log("selected exit location " + exitLocation);
        exitLocations.RemoveAt(exitLocations.Count-1);
        for (int i = 0; i < exitLocations.Count; i++)
        {
            int x = (int)exitLocations[i].x;
            int y = (int)exitLocations[i].y;
            tileData[x, y].cellType = MapCell.CellType.GrassFloor;
            tileData[x, y].interaction = null;

            //Debug.Log("setting " + x + " " + y + "to grass");
        }

    }
    */

}
