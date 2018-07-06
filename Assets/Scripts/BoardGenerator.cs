using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardGenerator : MonoBehaviour {

	public int boardHorizontalSize = 8;
	public int boardVerticalSize = 8;
    

	public int roomSize = 10;
	public int roomsOnPathDesired = 20;

    public Tilemap tilemap;



    public Vector2[] roomSequenceStartLocations;
	public RoomTemplate[] startRoomTemplates;
	public RoomTemplate[] randomFillRooms;
	public bool buildOnStart;
    public bool fillEmptySpaceWithRandomRooms;

    public List<Vector2> exitLocations;

	private RoomGenerator roomGenerator;
	private bool roomChainHitEdge;
	public List<Vector2> usedSpaces = new List<Vector2>();
	private Vector2 currentLocation;
	private int roomsOnPathCreated;
	private RoomTemplate currentRoom;

    public char[,] boardGridAsCharacters;
    private BoardInstantiator boardInstantiator;


    void Awake()
	{
		roomGenerator = GetComponent<RoomGenerator> ();
        boardInstantiator = GetComponent<BoardInstantiator>();
    }

    // Use this for initialization
    void Start () 
	{
        boardGridAsCharacters = new char[boardHorizontalSize, boardVerticalSize];

        if (buildOnStart)
        {
            BuildLevel();
        }
    }

	void BuildLevel()
	{
        boardInstantiator.Initialize();
		BuildBorder();
        if (fillEmptySpaceWithRandomRooms)
        {
            FillEmptySpaceWithRooms();
        }
		
		BuildRoomPath ();
        InstantiateGeneratedLevelData();
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

	public void FillEmptySpaceWithRooms()
	{
		int horizontalRoomsToFill = boardHorizontalSize / roomSize;
		int verticalRoomsToFill = boardVerticalSize / roomSize;
		for (int x = 0; x < horizontalRoomsToFill; x++) 
		{
			for (int y = 0; y < verticalRoomsToFill; y++) {
				Vector2 roomPos = new Vector2 (x * roomSize, y * roomSize);
				roomGenerator.ScriptableRoom(roomPos, randomFillRooms [Random.Range (0, randomFillRooms.Length)], 0, false);
			}
		}
	}

	public void BuildRoomPath()
	{
		Vector2 startLoc = roomSequenceStartLocations [Random.Range (0, roomSequenceStartLocations.Length)];
		RoomTemplate firstRoom = startRoomTemplates [Random.Range (0, startRoomTemplates.Length)];

        currentLocation = startLoc;
		currentRoom = firstRoom;

		for (int i = 0; i < 100; i++) 
		{
            if (!ChooseDirectionAndAddRoom())
            {
                Debug.Log("Roomgeneration terminated");
                break;
            }
			if (roomsOnPathCreated >= roomsOnPathDesired) 
			{
				Debug.Log ("created all desired rooms");
				break;
			}
				
		}
       // ChooseExit();


    }

	public bool ChooseDirectionAndAddRoom()
	{
		RoomAndDirection nextResult = currentRoom.ChooseNextRoom (this, currentLocation, usedSpaces);

		if (nextResult != null)
        {
			Vector2 nextLocation = nextResult.selectedDirection + currentLocation;
			RoomTemplate nextRoom = nextResult.selectedRoom;
			usedSpaces.Add (nextLocation);
			roomGenerator.Build (nextLocation, nextRoom, roomsOnPathCreated);
			roomsOnPathCreated++;
			currentRoom = nextRoom;
			currentLocation = nextLocation;
			return true;
		} else 
		{
			return false;
		}

	}

	public void BuildBorder()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = boardHorizontalSize + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = boardVerticalSize + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall (leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}

	void InstantiateVerticalOuterWall (float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            //InstantiateFromArray(wall, xCoord, currentY);
            SetTileFromGrid('w', (int)xCoord, (int)currentY); 
			currentY++;
		}
	}


	void InstantiateHorizontalOuterWall (float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            //InstantiateFromArray (wall, currentX, yCoord);
            SetTileFromGrid('w', (int)currentX, (int)yCoord);
            currentX++;
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

    void SetTileFromGrid(char charId, int x, int y)
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
