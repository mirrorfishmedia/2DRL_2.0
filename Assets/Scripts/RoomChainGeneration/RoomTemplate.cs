using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Templates/RoomTemplate")]
    public class RoomTemplate : ScriptableObject
    {
        public int roomSizeX = 10;
        public int roomSizeY = 10;

        public char[] roomChars = new char[100];

        public bool hasNorthExit;
        public bool hasEastExit;
        public bool hasSouthExit;
        public bool hasWestExit;

        public RoomAndDirection ChooseNextRoom(BoardGenerator boardGenerator, Vector2 currentLocation, List<Vector2> usedSpaces)
        {
            for (int z = 0; z < 400; z++)
            {

                List<RoomAndDirection> results = new List<RoomAndDirection>();
                results.Clear();
                RoomAndDirection result = new RoomAndDirection();

                int direction = Random.Range(0, 4);
                if (direction == 0 && hasNorthExit)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.movingNorthRoomTemplateList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.movingNorthRoomTemplateList.roomList.Count)];
                    Vector2 northDir = new Vector2(0, result.selectedChainRoom.roomSizeY);

                    if (SpaceValid(currentLocation + northDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = northDir;
                        results.Add(result);
                    }
                }

                if (direction == 2 && hasEastExit)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.movingEastRoomTemplateList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.movingEastRoomTemplateList.roomList.Count)];

                    Vector2 eastDir = new Vector2(result.selectedChainRoom.roomSizeX, 0);

                    if (SpaceValid(currentLocation + eastDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = eastDir;
                        results.Add(result);
                    }
                }

                if (direction == 1 && hasSouthExit)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.movingSouthRoomTemplateList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.movingSouthRoomTemplateList.roomList.Count)];

                    Vector2 southDir = new Vector2(0, -result.selectedChainRoom.roomSizeY);

                    if (SpaceValid(currentLocation + southDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = southDir;
                        results.Add(result);
                    }
                }

                if (direction == 3 && hasWestExit)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.movingWestRoomTemplateList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.movingWestRoomTemplateList.roomList.Count)];

                    Vector2 westDir = new Vector2(-result.selectedChainRoom.roomSizeX, 0);

                    if (SpaceValid(currentLocation + westDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = westDir;
                        results.Add(result);
                    }
                }

                if (results.Count != 0)
                {
                    RoomAndDirection selectedResult = results[Random.Range(0, results.Count)];
                    return selectedResult;
                }
            }

            return null;

        }

        bool SpaceValid(Vector2 spaceToTest, List<Vector2> usedSpaces, BoardGenerator generator)
        {
            if (usedSpaces.Contains(spaceToTest))
            {
                //space filled, cant build room
                return false;
            }
            else
            {
                if (spaceToTest.x < generator.profile.boardHorizontalSize && spaceToTest.y < generator.profile.boardVerticalSize && spaceToTest.x > 0 && spaceToTest.y > 0)
                {

                    //Space not filled, in board
                    return true;

                }
                else
                {
                    //Space out of board
                    return false;
                }
            }
        }
    }

}
