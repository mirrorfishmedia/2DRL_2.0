using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateRoomChain")]
    public class GeneratorRoomChain : Generator
    {

        public int roomSize = 10;
        public int roomsOnPathDesired = 20;
        public Vector2[] roomSequenceStartLocations;
        public RoomTemplate[] startRoomTemplates;
        public RoomTemplate[] randomFillRooms;
        public bool fillEmptySpaceWithRandomRooms;
        public bool fillBranchesOffChainWithRooms;


        public override void Generate(BoardGenerator boardGenerator)
        {
            StartRoomPath(boardGenerator);
            if (fillEmptySpaceWithRandomRooms)
            {
                FillEmptySpaceWithRooms(boardGenerator);
            }
            if (fillBranchesOffChainWithRooms)
            {

                AddRoomsToOpenDoors(boardGenerator);
            }
        }

        public void StartRoomPath(BoardGenerator boardGenerator)
        {
            Vector2 startLoc = roomSequenceStartLocations[Random.Range(0, roomSequenceStartLocations.Length)];
            RoomTemplate firstRoom = startRoomTemplates[Random.Range(0, startRoomTemplates.Length)];

            BuildRoomPath(boardGenerator, startLoc, firstRoom);
        }

        public void BuildRoomPath(BoardGenerator boardGenerator, Vector2 pathStartLoc, RoomTemplate startRoom)
        {

            boardGenerator.currentLocation = pathStartLoc;
            boardGenerator.currentChainRoom = startRoom;

            //WriteChainRoomToGrid(boardGenerator.currentLocation, boardGenerator.currentChainRoom, 0, true, boardGenerator);

            for (int i = 0; i < 100; i++)
            {
                if (!ChooseDirectionAndAddRoom(boardGenerator))
                {
                    //Ran out of space to create additional rooms, chain blocked.
                    Debug.Log("out of space ");
                    break;
                }
                if (boardGenerator.roomsOnPathCreated >= roomsOnPathDesired)
                {
                    //Created the requested number of rooms
                    Debug.Log("created requested rooms");
                    break;
                }

            }
            //ChooseExit();
        }

        public void AddRoomsToOpenDoors(BoardGenerator boardGenerator)
        {
            Debug.Log("locations pre removal " + boardGenerator.roomChainPathBranchLocations.Count);
            for (int i = 0; i < boardGenerator.roomChainRoomLocationsFilled.Count; i++)
            {
                Debug.Log("filled locations " + boardGenerator.roomChainRoomLocationsFilled.Count);
                for (int j = boardGenerator.branchDirections.Count - 1; j > -1; j--)
                {
                    Debug.Log("boardGenerator.branchDirections[j].offSetFromRoomLocation  " + boardGenerator.branchDirections[j].offSetFromRoomLocation);
                    GameObject placeHolder = new GameObject("holder " + j + " " + boardGenerator.branchDirections[j].selectedChainRoom.name);
                    placeHolder.transform.position = boardGenerator.branchDirections[j].offSetFromRoomLocation;
                    if (boardGenerator.branchDirections[j].offSetFromRoomLocation == boardGenerator.roomChainRoomLocationsFilled[i])
                    {
                        Debug.Log("removed  " + boardGenerator.branchDirections[j].offSetFromRoomLocation);
                        
                        //boardGenerator.branchDirections.RemoveAt(j);
                    }
                }
            }

            Debug.Log("pass");
            for (int i = 0; i < boardGenerator.branchDirections.Count; i++)
            {
                RoomAndDirection roomAndDir = boardGenerator.branchDirections[i];
                BuildRoomPath(boardGenerator, roomAndDir.offSetFromRoomLocation, roomAndDir.selectedChainRoom);
                Debug.Log("starting new path at: " + roomAndDir.selectedChainRoom + " " + i);
            }
        }

        public bool ChooseDirectionAndAddRoom(BoardGenerator boardGenerator)
        {
            RoomAndDirection nextResult = boardGenerator.currentChainRoom.ChooseNextRoom(boardGenerator, boardGenerator.currentLocation, boardGenerator.roomChainRoomLocationsFilled);

            if (nextResult != null)
            {
                Vector2 nextLocation = nextResult.selectedDirection + boardGenerator.currentLocation;
                RoomTemplate nextRoom = nextResult.selectedChainRoom;
                boardGenerator.roomChainRoomLocationsFilled.Add(nextLocation);
                WriteChainRoomToGrid(nextLocation, nextRoom, boardGenerator.roomsOnPathCreated, true, boardGenerator);
                boardGenerator.roomsOnPathCreated++;
                boardGenerator.currentChainRoom = nextRoom;
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
            int horizontalRoomsToFill = boardGenerator.profile.boardHorizontalSize / roomSize;
            int verticalRoomsToFill = boardGenerator.profile.boardVerticalSize / roomSize;
            for (int x = 0; x < horizontalRoomsToFill; x++)
            {
                for (int y = 0; y < verticalRoomsToFill; y++)
                {
                    Vector2 roomPos = new Vector2(x * roomSize, y * roomSize);
                    WriteChainRoomToGrid(roomPos, randomFillRooms[Random.Range(0, randomFillRooms.Length)], 0, false, boardGenerator);
                }
            }
        }

        public void WriteChainRoomToGrid(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath, BoardGenerator boardGenerator)
        {
#if UNITY_EDITOR
            //This generates GameObjects for each room in a room chain, it's useful in editor to be able to record the generation path and see if room generation is working correctly.
            //This does not run during the build of your game and can be safely removed if desired.
            if (isOnPath)
            {
                GameObject roomMarker = new GameObject("Path Room " + chainNumber + " " + roomTemplate.name);
                roomMarker.transform.position = roomOrigin;
                roomMarker.transform.SetParent(boardGenerator.transform);
            }
#endif

            int charIndex = 0;

            for (int i = 0; i < roomSize; i++)
            {
                for (int j = 0; j < roomSize; j++)
                {
                    char selectedChar = roomTemplate.roomChars[charIndex];
                    if (selectedChar != '\0')
                    {
                        Vector2 spawnPos = new Vector2(i, j) + roomOrigin;

                        int x = (int)spawnPos.x;
                        int y = (int)spawnPos.y;

                        //boardGenerator.boardGridAsCharacters[x, y] = selectedChar;
                        boardGenerator.WriteToBoardGrid(x, y, selectedChar, overwriteFilledSpaces);
                    }

                    charIndex++;

                }
            }
        }
    }
}