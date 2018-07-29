using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using Pathfinding;

public class WaypointCreator : MonoBehaviour {

    public int gridDimensionToSearch = 10;
    AstarPath astarPath;
    BoardGenerator boardGenerator;
    char emptyChar;

    private List<Vector2> waypointSpaces = new List<Vector2>();
    public List<Transform> waypoints = new List<Transform>();

    private StateController stateController;
	// Use this for initialization
	void Start ()
    {
        stateController = GetComponent<StateController>();
        astarPath = GameObject.FindObjectOfType<AstarPath>();
        boardGenerator = FindObjectOfType<BoardGenerator>();
        emptyChar = boardGenerator.profile.boardLibrary.GetDefaultEmptyChar();
        CreateWayPoints();

    }

    void CreateWayPoints()
    {
        SearchGridSquareForEmptySpaces();
        SpawnWaypoints();
        stateController.wayPointList = waypoints;

    }

    void SpawnWaypoints()
    {
        for (int i = 0; i < waypointSpaces.Count; i++)
        {
            GameObject waypoint = new GameObject("waypoint " + i);
            waypoint.transform.position = waypointSpaces[i];
            waypoints.Add(waypoint.transform);
        }
    }

    void SearchGridSquareForEmptySpaces()
    {
        Vector2 startPosition = transform.position;
        for (int x = 0; x < gridDimensionToSearch; x++)
        {
            for (int y = 0; y < gridDimensionToSearch; y++)
            {
                GridPosition gridPosition = new GridPosition((int)startPosition.x + x, (int)startPosition.y + y);
                bool spaceEmpty = boardGenerator.TestIfSpaceIsInGridAndMatchesChar(gridPosition, emptyChar);
                if (spaceEmpty)
                {
                    waypointSpaces.Add(gridPosition.GridPositionToVector2(gridPosition));
                }
            }
        }
    }
}
