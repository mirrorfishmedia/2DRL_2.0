using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    /// <summary>
    /// This is one of the main generators in Strata, it starts from an initial location in the top/northern row of the grid
    /// and then spawns RoomTemplates in sequence, randomly walking downward through the level. 
    /// It checks the doors you've labeled in the RoomTemplate and makes sure that each room spawned is connected. This is
    /// for generating connected vertical structures that are definitely connected and well suited to platformer games. It stops 
    /// when it reaches the bottom of the level. Then you can optionally fill the rest of the grid with random rooms.This approach is
    /// heavily inspired by the level generation in Derek Yu and Andy Hull's classic roguelike platformer Spelunky.
    ///  </summary>

    [CreateAssetMenu(menuName = "Strata/Generators/GeneratePlatformerRoomChain")]

    public class GeneratorPlatformerRoomChain : Generator
    {
        //Dimensions on x and y of the RoomTemplate, this has been tested with regular, equally sized rooms, YMMV with irregular rooms.
        public int roomSizeX = 10;
        public int roomSizeY = 10;


        //RoomLists of all rooms organized by their exits.
        public RoomList eastWestExits;
        public RoomList northSouthExits;
        public RoomList hasSouthExit;
        public RoomList hasNorthExit;

        //Set to true to fill the area not filled by the critical path from entrance to exit with random rooms
        public bool fillEmptySpaceWithRandomRooms = false;
        //Add rooms to this list to fill unused grid space randomly
        public RoomList randomFillRooms;

        //The RoomTemplate for the first room
        public RoomTemplate firstRoom;
        //The RoomTemplate for the last room, note if you want your player to progress bottom to top 
        // just swap this with firstRoom to have entrance on bottom, exit on top.
        public RoomTemplate lastRoom;

        //Enumeration for directions to improve code readability
        public enum Direction {North, East, South, West, NoMove};

        //This is the function called by BoardGenerator
        public override bool Generate(BoardGenerator boardGenerator)
        {
            //The number 999 is here to have a finite number of attempts to create a usable path but avoid infinite loops
            for (int i = 0; i < 999; i++)
            {
                if (BuildPath(boardGenerator))
                {
                    return true;
                }
            }
            return false;
        }

        //This is used to see if we have a valid space to place a RoomTemplate
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

            //Figure out how many rooms we will need to fill the board horizontally
            int horizontalRoomsToFill = boardGenerator.profile.boardHorizontalSize / roomSizeX;
            //Figure out how many rooms we need vertically
            int verticalRoomsToFill = boardGenerator.profile.boardVerticalSize / roomSizeY;

            //Create a two dimensional array of RoomTemplates in a grid based on the number of rooms
            RoomTemplate[,] roomTemplateGrid = new RoomTemplate[horizontalRoomsToFill,verticalRoomsToFill];

            //Create a variable to store the last direction we moved, this helps to avoid doubling back on the path
            Direction lastMoveDirection = Direction.NoMove;

            //Pick a random space in the room grid in the top row
            int startIndex = Random.Range(0, horizontalRoomsToFill);

            //Place first room in random position in top row
            roomTemplateGrid[startIndex, verticalRoomsToFill - 1] = firstRoom;

            //Did we change rows (move down) in the last loop?
            bool madeRowChange = true;

            //What column of our grid are we writing to? Set initially to the random one chosen in startIndex
            int columnIndex = startIndex;

            //Set our rowIndex to the top row in our grid
            int rowIndex = verticalRoomsToFill - 1;

            //Store the number of rooms created
            int roomsCreated;
            

            for (int i = 1; i < roomTemplateGrid.Length - 1; i++)
            {

                if (columnIndex > horizontalRoomsToFill || columnIndex < 0 || rowIndex < 0 || rowIndex > verticalRoomsToFill)
                {
                    break;
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

