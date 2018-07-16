using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/RoomTree")]
    public class GeneratorRoomTree : Generator
    {
        public int roomLocationsToGenerate = 10;
        public int roomSizeUniformXY = 10;
        public GridPosition startingPosition = new GridPosition(0, 0);
        public List<RoomTemplate> allRooms;


        public override bool Generate(BoardGenerator boardGenerator)
        {
            //List<GridPosition> potentialRoomLocations = new List<GridPosition>();

            //GeneratePotentialRoomLocations(boardGenerator, potentialRoomLocations);
            //BuildRooms(boardGenerator, potentialRoomLocations);
            return true;
            
        }

        void BuildRooms(BoardGenerator boardGenerator, List<GridPosition> potentialRoomLocations)
        {
            for (int i = 0; i < potentialRoomLocations.Count; i++)
            {
                Dictionary<GridPosition, List<RoomTemplate>> dictionary = BuildGridLocationDictionary(boardGenerator, potentialRoomLocations[i]);
                Debug.Log("dictionary count " + dictionary.Count);
                Debug.Log("GridPosition for i " + i + " " + potentialRoomLocations[i].x + " " + potentialRoomLocations[i].y);
                List<RoomTemplate> listForThisGridPosition = dictionary[potentialRoomLocations[i]];

                Debug.Log("listforthisgrid position count " + listForThisGridPosition.Count);
                if (listForThisGridPosition.Count != 0)
                {
                    RoomTemplate foundTemplate = listForThisGridPosition[Random.Range(0, listForThisGridPosition.Count)];
                    //WriteRoomTemplateToGrid(boardGenerator, foundTemplate, potentialRoomLocations[i]);
                }

            }
        }

        void GeneratePotentialRoomLocations(BoardGenerator boardGenerator, List<GridPosition> potentialRoomLocations)
        {
            
            for (int x = 0; x < roomLocationsToGenerate; x++)
            {
                for (int y = 0; y < roomLocationsToGenerate; y++)
                {
                    GridPosition roomOrigin = new GridPosition(x * roomSizeUniformXY, y * roomSizeUniformXY);
                    //Debug.Log("roomorigin " + roomOrigin.x + " , " + roomOrigin.y);
                    potentialRoomLocations.Add(roomOrigin);
                }
            }

            Debug.Log("potential room location count " + potentialRoomLocations.Count);
        }

        Dictionary<GridPosition, List<RoomTemplate>> BuildGridLocationDictionary(BoardGenerator boardGenerator, GridPosition gridPosition)
        {
            Dictionary<GridPosition, List<RoomTemplate>> positionRoomListDictionary = new Dictionary<GridPosition, List<RoomTemplate>>();

            

            if (!positionRoomListDictionary.ContainsKey(gridPosition))
            {
                //positionRoomListDictionary.Add(gridPosition, AddRoomsToListBasedOnPosition(boardGenerator, gridPosition));
            }

            return positionRoomListDictionary;
        }

        /*
        List<RoomTemplate> AddRoomsToListBasedOnPosition(BoardGenerator boardGenerator, GridPosition gridPosition)
        {
            List<RoomTemplate> allowedRooms = new List<RoomTemplate>();
            for (int j = 0; j < allRooms.Count; j++)
            {
                allowedRooms.Add(allRooms[j]);
            }

            Debug.Log("allowedRooms.Count " + allowedRooms.Count);

            for (int i = allowedRooms.Count - 1; i > -1; i--)
            {
                Debug.Log("i " + i);
                if (gridPosition.x == 0)
                {
                    Debug.Log("pass");
                    if (allowedRooms[i].opensToWest)
                    {
                        allowedRooms.RemoveAt(i);
                    }
                }

                if (gridPosition.x == boardGenerator.profile.boardHorizontalSize - roomSizeUniformXY)
                {
                    if (allowedRooms[i].opensToEast)
                    {
                        allowedRooms.RemoveAt(i);

                    }
                }

                if (gridPosition.y == 0)
                {
                    if (allowedRooms[i].opensToSouth)
                    {
                        allowedRooms.RemoveAt(i);

                    }
                }

                if (gridPosition.y == boardGenerator.profile.boardVerticalSize - roomSizeUniformXY)
                {
                    if (allowedRooms[i].opensToNorth)
                    {
                        allowedRooms.RemoveAt(i);

                    }
                }
            }

            Debug.Log("grid position testing " + gridPosition.x + " " + gridPosition.y + " allowed rooms count " + allowedRooms.Count);

            return allowedRooms;
        }

        void WriteRoomTemplateToGrid(BoardGenerator boardGenerator, RoomTemplate roomTemplate, GridPosition roomOrigin)
        {
            int charIndex = 0;

            GameObject roomMarker = new GameObject(roomTemplate.name);
            roomMarker.transform.position = new Vector2(roomOrigin.x, roomOrigin.y);

            for (int i = 0; i < roomTemplate.roomSizeX; i++)
            {
                for (int j = 0; j < roomTemplate.roomSizeY; j++)
                {
                    char selectedChar = roomTemplate.roomChars[charIndex];
                    if (selectedChar != '\0')
                    {
                        Vector2 spawnPos = new Vector2(i, j) + new Vector2(roomOrigin.x, roomOrigin.y);

                        int x = (int)spawnPos.x;
                        int y = (int)spawnPos.y;

                        //boardGenerator.boardGridAsCharacters[x, y] = selectedChar;
                        boardGenerator.WriteToBoardGrid(x, y, selectedChar, overwriteFilledSpaces);
                    }

                    charIndex++;

                }
            }
        }
        */
    }

}
