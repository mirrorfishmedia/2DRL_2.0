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

    [HideInInspector]
    public List<Vector2> exitLocations;


    [HideInInspector]
    public List<Vector2> roomChainRoomLocationsFilled = new List<Vector2>();
    [HideInInspector]
    public Vector2 currentLocation;
    [HideInInspector]
    public int roomsOnPathCreated;
    [HideInInspector]
    public RoomTemplate currentRoom;

    public char[,] boardGridAsCharacters;
    private Dictionary<char, BoardLibraryEntry> libraryDictionary;
    

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
        InitializeLibraryDictionary();
        boardLibrary.Initialize();
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

    public void InitializeLibraryDictionary()
    {
        libraryDictionary = new Dictionary<char, BoardLibraryEntry>();
        for (int i = 0; i < boardLibrary.boardLibraryEntries.Length; i++)
        {
            libraryDictionary.Add(boardLibrary.boardLibraryEntries[i].characterId, boardLibrary.boardLibraryEntries[i]);
        }
    }

    public BoardLibraryEntry GetLibraryEntryViaCharacterId(char charId)
    {
        BoardLibraryEntry entry = null;
        if (libraryDictionary.ContainsKey(charId))
        {
            entry = libraryDictionary[charId];
        }
        else
        {
            if (charId == '\0')
            {
                return boardLibrary.GetDefaultEntry();
            }

            Debug.LogError("Attempt to get charId " + charId.ToString() + " from boardLibrary failed, is there an entry with that ID in the boardLibrary?");
        }

        return entry;
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
                CreateMapEntryFromGrid(boardGridAsCharacters[x, y],spawnPos);
            }
        }
    }

    bool SpaceValid(Vector2 spaceToTest)
	{
		if (roomChainRoomLocationsFilled.Contains(spaceToTest))
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

    public void CreateMapEntryFromGrid(char charId, Vector2 position)
    {
        BoardLibraryEntry entryToSpawn = GetLibraryEntryViaCharacterId(charId);
        boardLibrary.instantiationTechnique.SpawnBoardSquare(this, position, entryToSpawn);
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
