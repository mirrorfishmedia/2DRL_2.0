using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Generators/GeneratePlatformerRoomChain")]

    public class GeneratorPlatformerRoomChain : Generator
    {
        public int roomSizeX = 12;
        public int roomSizeY = 10;

        public RoomList eastWestExits;
        public RoomList northSouthExits;
        public RoomList hasSouthExit;
        public RoomList hasNorthExit;


        public bool fillEmptySpaceWithRandomRooms = false;
        public RoomList randomFillRooms;

        public RoomTemplate firstRoom;
        public RoomTemplate lastRoom;

        public enum Direction {North, East, South, West, NoMove};

        public override bool Generate(BoardGenerator boardGenerator)
        {
            for (int i = 0; i < 999; i++)
            {
                if (BuildPath(boardGenerator))
                {
                    return true;
                }
            }
            return false;
        }

        bool TestIfGridIndexIsValid(int x, int y, int gridWidthX, int gridWidthY)
        {
            if (x > gridWidthX-1 || x < 0 || y > gridWidthY-1 || gridWidthY < 0 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        bool BuildPath(BoardGenerator boardGenerator)
        {
            Debug.Log("**Starting Path Build**");

            bool pathBuildComplete = false;
            bool lastRoomPlaced = false;

            int horizontalRoomsToFill = boardGenerator.profile.boardHorizontalSize / roomSizeX;
            int verticalRoomsToFill = boardGenerator.profile.boardVerticalSize / roomSizeY;

            RoomTemplate[,] roomTemplateGrid = new RoomTemplate[horizontalRoomsToFill,verticalRoomsToFill];

            Direction lastMoveDirection = Direction.NoMove;

            
            int startIndex = Random.Range(0, horizontalRoomsToFill);

            //Place first room in random position in top row
            roomTemplateGrid[startIndex, verticalRoomsToFill - 1] = firstRoom;

            Debug.Log("startIndex = " + startIndex + " " + (verticalRoomsToFill - 1));

            bool madeRowChange = true;
            int columnIndex = startIndex;
            int rowIndex = verticalRoomsToFill - 1;
            int roomsCreated;
            List<int> moveDirInts = new List<int>();

            for (int i = 1; i < roomTemplateGrid.Length - 1; i++)
            {

                if (columnIndex > horizontalRoomsToFill || columnIndex < 0 || rowIndex < 0 || rowIndex > verticalRoomsToFill)
                {
                    break;
                }

                if (madeRowChange)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        moveDirInts.Add(j);
                    }
                    madeRowChange = false;
                }

                int randomDir = 0;
                if (i == 1)
                {
                    randomDir = Random.Range(0, 4);
                }
                else
                {
                    randomDir = Random.Range(0, 5);
                }

                bool roomCreated = false;

                roomsCreated = i;
                switch (randomDir)
                {
                    case 0:
                    case 1:


                        //Move Right
                        if (TestIfGridIndexIsValid(columnIndex + 1, rowIndex, horizontalRoomsToFill, verticalRoomsToFill) && lastMoveDirection != Direction.West)
                        {
                            Debug.Log("move right");
                            RoomTemplate room = eastWestExits.roomList[Random.Range(0, eastWestExits.roomList.Count)];
                            Debug.Log("room " + i + "  index " + (columnIndex + 1) + " " + rowIndex);
                            if (roomTemplateGrid[columnIndex + 1, rowIndex] == null)
                            {
                                roomTemplateGrid[columnIndex + 1, rowIndex] = room;
                                columnIndex = columnIndex + 1;
                                roomCreated = true;
                                lastMoveDirection = Direction.East;
                            }
                                
                        }
                        else
                        {
                            Debug.Log("invalid right move attempted");

                            if (TestIfGridIndexIsValid(columnIndex + 1, rowIndex, horizontalRoomsToFill, verticalRoomsToFill) && lastMoveDirection != Direction.West)
                            {
                                Debug.Log("move right");
                                RoomTemplate room = eastWestExits.roomList[Random.Range(0, eastWestExits.roomList.Count)];
                                Debug.Log("room " + i + "  index " + (columnIndex + 1) + " " + rowIndex);
                                if (roomTemplateGrid[columnIndex + 1, rowIndex] == null)
                                {
                                    roomTemplateGrid[columnIndex + 1, rowIndex] = room;
                                    columnIndex = columnIndex + 1;
                                    roomCreated = true;
                                    lastMoveDirection = Direction.East;
                                }

                            }

                        }
                        break;

                    case 2:
                    case 3:
                        
                        //Move Left
                        if (TestIfGridIndexIsValid(columnIndex - 1, rowIndex, horizontalRoomsToFill, verticalRoomsToFill) && lastMoveDirection != Direction.East)
                        {
                            //lastMoveDirection = MoveGenerator(BoardGenerator boardGenerator, columnIndex - 1, rowIndex, roomTemplateGrid, eastWestExits);
                            Debug.Log("move left");
                            RoomTemplate room = eastWestExits.roomList[Random.Range(0, eastWestExits.roomList.Count)];
                            Debug.Log("room" + i + "  index " + (columnIndex - 1) + " " + rowIndex);
                            if (roomTemplateGrid[columnIndex - 1, rowIndex] == null)
                            {
                                roomTemplateGrid[columnIndex - 1, rowIndex] = room;
                                columnIndex = columnIndex - 1;
                                roomCreated = true;
                                lastMoveDirection = Direction.West;
                            }
                            
                        }
                        else
                        {
                            if (TestIfGridIndexIsValid(columnIndex - 1, rowIndex, horizontalRoomsToFill, verticalRoomsToFill) && lastMoveDirection != Direction.East)
                            {
                                //lastMoveDirection = MoveGenerator(BoardGenerator boardGenerator, columnIndex - 1, rowIndex, roomTemplateGrid, eastWestExits);
                                Debug.Log("move left");
                                RoomTemplate room = eastWestExits.roomList[Random.Range(0, eastWestExits.roomList.Count)];
                                Debug.Log("room" + i + "  index " + (columnIndex - 1) + " " + rowIndex);
                                if (roomTemplateGrid[columnIndex - 1, rowIndex] == null)
                                {
                                    roomTemplateGrid[columnIndex - 1, rowIndex] = room;
                                    columnIndex = columnIndex - 1;
                                    roomCreated = true;
                                    lastMoveDirection = Direction.West;
                                }
                            }
                        }
                        break;
                    case 4:
                        Debug.Log("move down");
                        if (TestIfGridIndexIsValid(columnIndex, rowIndex-1, horizontalRoomsToFill, verticalRoomsToFill) && lastMoveDirection != Direction.South)
                        {
                            RoomTemplate room = hasSouthExit.roomList[Random.Range(0, hasSouthExit.roomList.Count)];

                            roomTemplateGrid[columnIndex, rowIndex] = room;
                            lastMoveDirection = Direction.South;

                           
                            Debug.Log("after moved down room " + i + " index " + (columnIndex) + " " + rowIndex);
                            if (rowIndex > 0)
                            {
                                rowIndex--;
                                RoomTemplate roomWithNorthExit = hasNorthExit.roomList[Random.Range(0, hasNorthExit.roomList.Count)];
                                Debug.Log("spawning lower room " + roomWithNorthExit);

                                roomTemplateGrid[columnIndex, rowIndex] = roomWithNorthExit;
                            }
                            else
                            {
                                if (!lastRoomPlaced)
                                {
                                    roomTemplateGrid[columnIndex, rowIndex] = lastRoom;
                                    lastRoomPlaced = true;
                                    break;
                                }
                                
                            }
                            roomCreated = true;
                            madeRowChange = true;
                        }
                        break;

                    default:

                        Debug.Log("default");
                        break;
                }

                if (roomCreated == false)
                {
                    Debug.Log("room created outside switch, moving down, column index " + columnIndex + " row index "+ rowIndex);
                    roomTemplateGrid[columnIndex, rowIndex] = northSouthExits.roomList[Random.Range(0, northSouthExits.roomList.Count)];
                    lastMoveDirection = Direction.South;

                    if (rowIndex > 0)
                    {
                        rowIndex--;
                        roomTemplateGrid[columnIndex, rowIndex] = hasNorthExit.roomList[Random.Range(0, hasNorthExit.roomList.Count)];
                    }
                    else
                    {
                        Debug.Log(" else " + lastRoomPlaced);
                        if (!lastRoomPlaced)
                        {
                            
                            roomTemplateGrid[columnIndex, rowIndex] = lastRoom;
                            lastRoomPlaced = true;
                            Debug.Log(" pass " + lastRoomPlaced);
                            break;
                        }
                    }
                        
                    roomCreated = true;
                    madeRowChange = true;
                }
            }            

            for (int j = 0; j < horizontalRoomsToFill; j++)
            {
                for (int k = 0; k < verticalRoomsToFill; k++)
                {
                    if (roomTemplateGrid[j, k] == firstRoom)
                    {
                        pathBuildComplete = true;
                    }
                }
            }

          

            if (fillEmptySpaceWithRandomRooms)
            {
                FillGridWithRandomRooms(boardGenerator, roomTemplateGrid, horizontalRoomsToFill, verticalRoomsToFill);

            }

            int roomChainNumber = 0;

            for (int x = 0; x < horizontalRoomsToFill; x++)
            {
                for (int y = 0; y < verticalRoomsToFill; y++)
                {
                   
                    Vector2 roomPos = new Vector2(x * roomSizeX, y * roomSizeY);
                    RoomTemplate templateToWrite = roomTemplateGrid[x, y];
                    if (templateToWrite != null)
                    {
                        roomChainNumber++;
                        boardGenerator.GenerateRoomPlaceHolderGameObject(boardGenerator, roomPos, templateToWrite, roomChainNumber, true, "platform room ");
                        WriteChainRoomToGrid(boardGenerator, roomPos, templateToWrite, roomChainNumber, true);
                    }
                    
                }
            }

            return pathBuildComplete;
        }


        public void WriteChainRoomToGrid(BoardGenerator boardGenerator, Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath)
        {
#if UNITY_EDITOR
            //This generates GameObjects for each room in a room chain, it's useful in editor to be able to record the generation path and see if room generation is working correctly.
            //This does not run during the build of your game and can be safely removed if desired.

            boardGenerator.GenerateRoomPlaceHolderGameObject(boardGenerator, roomOrigin, roomTemplate, chainNumber, isOnPath, "Chain Room");

#endif

            int charIndex = 0;

            for (int i = 0; i < roomSizeX; i++)
            {
                for (int j = 0; j < roomSizeY; j++)
                {
                    char selectedChar = roomTemplate.roomChars[charIndex];
                    if (selectedChar != '\0')
                    {
                        Vector2 spawnPos = new Vector2(i, j) + roomOrigin;

                        int x = (int)spawnPos.x;
                        int y = (int)spawnPos.y;

                        //boardGenerator.boardGridAsCharacters[x, y] = selectedChar;
                        boardGenerator.WriteToBoardGrid(x, y, selectedChar, overwriteFilledSpaces, isOnPath);
                    }

                    charIndex++;

                }
            }
        }

        public void FillGridWithRandomRooms(BoardGenerator boardGenerator, RoomTemplate[,] roomTemplateGrid, int horizontalRoomsToFill, int verticalRoomsToFill)
        {
            for (int x = 0; x < horizontalRoomsToFill; x++)
            {
                for (int y = 0; y < verticalRoomsToFill; y++)
                {
                    if (roomTemplateGrid[x,y] == null)
                    {

                        RoomTemplate selectedRoom = randomFillRooms.roomList[Random.Range(0, randomFillRooms.roomList.Count)];
                        roomTemplateGrid[x, y] = selectedRoom;

                    }

                }
            }
        }

    }
}

