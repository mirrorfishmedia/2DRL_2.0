using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardGenerator : MonoBehaviour {

    public bool buildOnStart;

    public int boardHorizontalSize = 8;
	public int boardVerticalSize = 8;
    public Tilemap tilemap;
    public BoardLibrary boardLibrary;

    public Generator[] generators;
    public char emptySpaceChar = '0';

    public List<Vector2> exitLocations;

	private bool roomChainHitEdge;
	public List<Vector2> usedRoomAreas = new List<Vector2>();
	public Vector2 currentLocation;
    public int roomsOnPathCreated;
	public RoomTemplate currentRoom;

    public char[,] boardGridAsCharacters;
    private BoardInstantiator boardInstantiator;


    public GeneratorSpaceList[] generatorSpaceLists;

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
        SetupGeneratorSpaceLists();
        boardInstantiator.Initialize();
        boardLibrary.Initialize();
        SetupEmptyGrid();
        RunGenerators();
        InstantiateGeneratedLevelData();
    }


    void SetupGeneratorSpaceLists()
    {
        generatorSpaceLists = new GeneratorSpaceList[generators.Length];
        for (int i = 0; i < generatorSpaceLists.Length; i++)
        {
            generatorSpaceLists[i] = new GeneratorSpaceList();
            generatorSpaceLists[i].generatorType = generators[i];
            generatorSpaceLists[i].Initialize();

        }

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
		if (usedRoomAreas.Contains(spaceToTest))
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

    public void SpawnChanceTile()
    {

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

    public bool TestIfTileTraversable(char charId)
    {
        for (int i = 0; i < boardLibrary.boardLibraryEntries.Length; i++)
        {
            if (boardLibrary.boardLibraryEntries[i].characterId == charId)
            {
                if (boardLibrary.boardLibraryEntries[i].traversable > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void WriteToGeneratorSpaceList(GridPosition positionToWrite, Generator generator)
    {
        for (int i = 0; i < generatorSpaceLists.Length; i++)
        {

            //if (generatorSpaceLists[i].generatorType == generator)
            //{
                //Debug.Log("generatorSpaceLists[i].generatorType " + generatorSpaceLists[i].generatorType);
                generatorSpaceLists[i].positionList.Add(positionToWrite);
            //}
        }
    }

    public void WriteToBoardGrid(int x, int y, char charIdToWrite, bool overwriteFilledSpaces)
    {
        if (TestIfInGrid(x, y))
        {
            if (overwriteFilledSpaces)
            {
                char nextChar = boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                boardGridAsCharacters[x, y] = nextChar;
            }
            else
            {
                if (boardGridAsCharacters[x, y] == emptySpaceChar)
                {
                    char nextChar = boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                    boardGridAsCharacters[x, y] = nextChar;
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
