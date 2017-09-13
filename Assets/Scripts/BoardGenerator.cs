using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public int boardHorizontalSize = 8;
	public int boardVerticalSize = 8;

	public int cameraFrustumX = 26;
	public int cameraFrustumY = 18;

	public int roomSize = 10;
	public int roomsOnPathDesired = 20;
	public GameObject exitPrefab;
	public GameObject blackFloor;
	public GameObject grassFloor;
	public GameObject exit;

	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject coin;
	public GameObject mushroom;
	public GameObject wall;

	public Vector2[] roomSequenceStartLocations;
	public RoomTemplate[] startRoomTemplates;
	public RoomTemplate[] randomFillRooms;
	public bool buildOnStart;
	public Transform player;

	public Tile[,] tileData;


	private GameObject boardHolder;
	private List<GameObject> objectsInLevel = new List<GameObject>();
	private RoomGenerator roomGenerator;
	private bool roomChainHitEdge;
	public List<Vector2> usedSpaces = new List<Vector2>();
	private Vector2 currentLocation;
	private int roomsOnPathCreated;
	private RoomTemplate currentRoom;


	void Awake()
	{
		roomGenerator = GetComponent<RoomGenerator> ();
	}

	// Use this for initialization
	void Start () 
	{
		//boardData = new int[boardHorizontalSize,boardVerticalSize];
		tileData = new Tile[boardHorizontalSize, boardVerticalSize];
		for (int x = 0; x < boardHorizontalSize; x++) {
			for (int y = 0; y < boardVerticalSize; y++) {
				tileData [x, y] = new Tile ();
			}
		}
		if (buildOnStart)
		BuildLevel ();
	}

	void BuildLevel()
	{
		boardHolder = new GameObject("BoardHolder");
		BuildBorder();
		FillEmptySpaceWithRooms ();
		BuildRoomPath ();
		//BuildGameObjectsFromGrid ();
		BuildGridInFrustum((Vector2) player.position);
	}
	
	void Update () 
	{
		if (Input.GetButtonDown ("Jump")) {
			ClearAndRebuild ();
		}
	}

	public void TrackMovingUnit(Vector2 unitCoordinates, Tile.TileType tileType)
	{
		int unitX = (int)unitCoordinates.x;
		int unitY = (int)unitCoordinates.y;
		//boardData [unitX, unitY] = unitID;
		tileData [unitX, unitY].tileType = tileType;
		ClearInstantiated ();
		BuildGridInFrustum (player.position);
	}

	public bool CheckMapForObstruction(int x, int y)
	{
		if (TestIfInGrid(x,y)) 
		{
			Tile targetTile = tileData [x, y];

			if (targetTile.tileType == Tile.TileType.BlackFloor) {
				return true;
			} else 
			{
				if (targetTile.interaction != null) 
				{
					targetTile.interaction.RespondToInteraction (targetTile);
					ClearInstantiated ();
					BuildGridInFrustum (player.position);
				}
			}
		}

		return false;
	}


	void ClearAndRebuild()
	{
		roomsOnPathCreated = 0;
		usedSpaces.Clear ();
		for (int i = objectsInLevel.Count - 1; i >= 0 ; i--) 
		{
			GameObject toDestroy = objectsInLevel [i];
			objectsInLevel.Remove (objectsInLevel [i]);
			Destroy (toDestroy);
		}
		BuildLevel ();
	}

	void ClearInstantiated()
	{
		for (int i = objectsInLevel.Count - 1; i >= 0 ; i--) 
		{
			GameObject toDestroy = objectsInLevel [i];
			objectsInLevel.Remove (objectsInLevel [i]);
			Destroy (toDestroy);
		}
	}

	public void TrackInstantiatedObject(GameObject trackedObject)
	{
		objectsInLevel.Add (trackedObject);
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
				roomGenerator.StringToSquare (roomPos, randomFillRooms [Random.Range (0, randomFillRooms.Length)], 0, false);
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
			//Debug.Log ("loop " + i);
			ChooseDirection ();
			if (roomsOnPathCreated >= roomsOnPathDesired) 
			{
				Debug.Log ("created all desired rooms");
				break;
			}
				
		}

	}

	public bool ChooseDirection()
	{
		RoomAndDirection nextResult = currentRoom.ChooseNextRoom (this, currentLocation, usedSpaces);


		if (nextResult != null) {
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
			InstantiateFromArray(wall, xCoord, currentY);

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
			InstantiateFromArray (wall, currentX, yCoord);

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
	public void BuildGridInFrustum(Vector2 playerPos)
	{
		int playerX = (int)playerPos.x;
		int playerY = (int)playerPos.y;

		int frustumStartX = playerX - (cameraFrustumX / 2);
		int frustumStartY = playerY - (cameraFrustumY / 2);

		for (int x = frustumStartX; x < frustumStartX + cameraFrustumX; x++) {
			for (int y = frustumStartY; y < frustumStartY + cameraFrustumY; y++) 
			{
				//int boardDataValue = boardData [x, y];
				if (TestIfInGrid (x,y)) 
				{
					Tile.TileType tileDataValue = tileData[x,y].tileType;
					switch (tileDataValue) 
					{
					case Tile.TileType.BlackFloor:
						InstantiateFromArray (blackFloor, x, y);
						break;
					case Tile.TileType.Wall:
						InstantiateFromArray (wall, x, y);
						break;
					case Tile.TileType.Exit:
						InstantiateFromArray (exit, x, y);
						break;
					case Tile.TileType.Coin:
						Debug.Log ("generating treasure");
						InstantiateFromArray (coin, x, y);
						break;
					case Tile.TileType.Enemy1:
						InstantiateFromArray (enemy1, x, y);
						break;
					case Tile.TileType.Obstacle:
						InstantiateFromArray (wall, x, y);
						break;

					}
				}

			}
		}
	}
		
	void InstantiateFromArray (GameObject prefab, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		//int randomIndex = Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = Instantiate(prefab, position, Quaternion.identity) as GameObject;

		// Set the tile's parent to the board holder.
		tileInstance.transform.parent = boardHolder.transform;
		TrackInstantiatedObject (tileInstance);
	}
}
