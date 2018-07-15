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

        public bool opensToNorth;
        public bool opensToEast;
        public bool opensToSouth;
        public bool opensToWest;

        void OnValidate()
        {
            if (roomChars.Length != roomSizeX * roomSizeY)
            {
                char[] newSizedArray = new char[roomSizeX * roomSizeY];
                roomChars = newSizedArray;
            }
        }

        public RoomAndDirection ChooseNextRoom(BoardGenerator boardGenerator, Vector2 currentLocation, List<Vector2> usedSpaces)
        {
            for (int z = 0; z < 400; z++)
            {

                List<RoomAndDirection> results = new List<RoomAndDirection>();
                results.Clear();
                RoomAndDirection result = new RoomAndDirection();

                if (opensToNorth)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.canBeEnteredFromNorthList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.canBeEnteredFromNorthList.roomList.Count)];
                    Vector2 northDir = new Vector2(0, result.selectedChainRoom.roomSizeY);

                    if (SpaceValid(currentLocation + northDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = northDir;
                        result.offSetFromRoomLocation = currentLocation + result.selectedDirection;
                        results.Add(result);
                    }
                }

                if (opensToEast)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.canBeEnteredFromEastList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.canBeEnteredFromEastList.roomList.Count)];

                    Vector2 eastDir = new Vector2(result.selectedChainRoom.roomSizeX, 0);

                    if (SpaceValid(currentLocation + eastDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = eastDir;
                        result.offSetFromRoomLocation = currentLocation + result.selectedDirection;
                        results.Add(result);
                    }
                }

                if (opensToSouth)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.canBeEnteredFromSouthList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.canBeEnteredFromSouthList.roomList.Count)];

                    Vector2 southDir = new Vector2(0, -result.selectedChainRoom.roomSizeY);

                    if (SpaceValid(currentLocation + southDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = southDir;
                        result.offSetFromRoomLocation = currentLocation + result.selectedDirection;
                        results.Add(result);
                    }
                }

                if (opensToWest)
                {
                    result.selectedChainRoom = boardGenerator.profile.boardLibrary.canBeEnteredFromWestList.roomList[Random.Range(0, boardGenerator.profile.boardLibrary.canBeEnteredFromWestList.roomList.Count)];

                    Vector2 westDir = new Vector2(-result.selectedChainRoom.roomSizeX, 0);

                    if (SpaceValid(currentLocation + westDir, usedSpaces, boardGenerator))
                    {
                        result.selectedDirection = westDir;
                        result.offSetFromRoomLocation = currentLocation + result.selectedDirection;
                        results.Add(result);
                    }
                }

                if (results.Count != 0)
                {
                    RoomAndDirection selectedResult = results[Random.Range(0, results.Count)];
                    
                    results.Remove(selectedResult);
                    //Debug.Log("Remaining results count " + results.Count);
                    if (results.Count != 0)
                    {
                        for (int i = 0; i < results.Count; i++)
                        {       
                            boardGenerator.branchDirections.Add(results[i]);
                        }
                    }

                    //Debug.Log("selected result roomchain: " + selectedResult.selectedChainRoom);

                    for (int j = 0; j < boardGenerator.branchDirections.Count; j++)
                    {
                        //Debug.Log("branchDirections " + boardGenerator.branchDirections[j].selectedChainRoom + " dir " + boardGenerator.branchDirections[j].selectedDirection);
                    }
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
