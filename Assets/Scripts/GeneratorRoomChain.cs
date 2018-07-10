using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateRoomChain")]

public class GeneratorRoomChain : Generator
{

    public int roomSize = 10;
    public int roomsOnPathDesired = 20;
    public Vector2[] roomSequenceStartLocations;
    public RoomTemplate[] startRoomTemplates;
    public RoomTemplate[] randomFillRooms;
    public bool fillEmptySpaceWithRandomRooms;

    public override void Generate(BoardGenerator boardGenerator)
    {
        BuildRoomPath(boardGenerator);
        if(fillEmptySpaceWithRandomRooms)
        {
            FillEmptySpaceWithRooms(boardGenerator);
        }
    }

    public void BuildRoomPath(BoardGenerator boardGenerator)
    {
        Vector2 startLoc = roomSequenceStartLocations[Random.Range(0, roomSequenceStartLocations.Length)];
        RoomTemplate firstRoom = startRoomTemplates[Random.Range(0, startRoomTemplates.Length)];

        boardGenerator.currentLocation = startLoc;
        boardGenerator.currentRoom = firstRoom;

        ScriptableRoom(boardGenerator.currentLocation, boardGenerator.currentRoom, 0,true,boardGenerator);

        for (int i = 0; i < 100; i++)
        {
            if (!ChooseDirectionAndAddRoom(boardGenerator))
            {
                Debug.Log("Roomgeneration terminated");
                break;
            }
            if (boardGenerator.roomsOnPathCreated >= roomsOnPathDesired)
            {
                Debug.Log("created all desired rooms");
                break;
            }

        }
        //ChooseExit();
    }

    public bool ChooseDirectionAndAddRoom(BoardGenerator boardGenerator)
    {
        RoomAndDirection nextResult = boardGenerator.currentRoom.ChooseNextRoom(boardGenerator, boardGenerator.currentLocation, boardGenerator.usedRoomAreas);

        if (nextResult != null)
        {
            Vector2 nextLocation = nextResult.selectedDirection + boardGenerator.currentLocation;
            RoomTemplate nextRoom = nextResult.selectedRoom;
            boardGenerator.usedRoomAreas.Add(nextLocation);
            ScriptableRoom(nextLocation, nextRoom, boardGenerator.roomsOnPathCreated, true, boardGenerator);
            boardGenerator.roomsOnPathCreated++;
            boardGenerator.currentRoom = nextRoom;
            boardGenerator.currentLocation = nextLocation;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void FillEmptySpaceWithRooms(BoardGenerator boardGenerator)
    {
        int horizontalRoomsToFill = boardGenerator.boardHorizontalSize / roomSize;
        int verticalRoomsToFill = boardGenerator.boardVerticalSize / roomSize;
        for (int x = 0; x < horizontalRoomsToFill; x++)
        {
            for (int y = 0; y < verticalRoomsToFill; y++)
            {
                Vector2 roomPos = new Vector2(x * roomSize, y * roomSize);
                ScriptableRoom(roomPos, randomFillRooms[Random.Range(0, randomFillRooms.Length)], 0, false, boardGenerator);
            }
        }
    }

    public void ScriptableRoom(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath, BoardGenerator boardGenerator)
    {

        if (isOnPath)
        {
            GameObject roomHolder = new GameObject("Path Room " + chainNumber + " " + roomTemplate.name);
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

                    GridPosition spawnGrid = new GridPosition((int)spawnPos.x, (int)spawnPos.y);
                    
                    boardGenerator.WriteToBoardGrid(spawnGrid.x, spawnGrid.y, selectedChar, true);
                    if (boardGenerator.TestIfTileTraversable(selectedChar))
                    {
                        boardGenerator.WriteToGeneratorSpaceList(spawnGrid, this);
                    }
                }

                charIndex++;

            }
        }
    }
}
