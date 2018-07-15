using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GeneratorTunneler")]
    public class GeneratorTunneler : Generator
    {
        
        
        public int numTunnels = 4;
        public int tunnelWidth = 1;
        public RoomTemplate[] tunnelEndTemplates;
        public bool spawnRoomsAtTunnelEnds = false;
        public bool overWriteFilledCharacters = true;
        public bool useRandomTunnelStartPositions = false;
        public bool connectLastStrataLayer = true;

        public bool useCustomEmptySpaceCharForTunnels = false;
        public char customEmptySpaceChar = '0';


        public override void Generate(BoardGenerator boardGenerator)
        {
            GridPosition startPos = new GridPosition(0,0);

            if (useRandomTunnelStartPositions)
            {
                startPos = boardGenerator.GetRandomGridPosition();
                for (int i = 0; i < numTunnels; i++)
                {
                    GridPosition randomGoalPosition = boardGenerator.GetRandomGridPosition();
                    DigTunnel(boardGenerator, startPos, randomGoalPosition);
                }
            }

            else if(connectLastStrataLayer)
            {
                List<GridPosition> goalPositions = BuildTunnelGoalList(boardGenerator);
                

                for (int i = 0; i < goalPositions.Count; i++)
                {

                    startPos = goalPositions[i];
                    int loopingGoalPositionIndex = ((i + 1) % goalPositions.Count);
                    GridPosition targetPosition = goalPositions[loopingGoalPositionIndex];
                    DigTunnel(boardGenerator, startPos, targetPosition);
                }
            }
            
            
        }

        private List<GridPosition> BuildTunnelGoalList(BoardGenerator boardGenerator)
        {
            List<GridPosition> goalPositions = new List<GridPosition>();

            for (int i = 0; i <= boardGenerator.currentGeneratorIndexIdForEmptySpaceTracking; i++)
            {
                for (int j = 0; j < numTunnels; j++)
                {
                    if (boardGenerator.emptySpaceLists[i].gridPositionList.Count > 0)
                    {
                        int index = Random.Range(0, boardGenerator.emptySpaceLists[i].gridPositionList.Count);
                        GridPosition emptyPosition = boardGenerator.emptySpaceLists[i].gridPositionList[index];
                        boardGenerator.emptySpaceLists[i].gridPositionList.RemoveAt(index);
                        goalPositions.Add(emptyPosition);
                    }
                }
            }

            return goalPositions;
        }

        public void DigTunnel(BoardGenerator boardGenerator, GridPosition startPosition, GridPosition tunnelGoal)
        {
            GridPosition currentDigPosition = startPosition;

            for (int i = 0; i < 1000; i++)
            {

                if (currentDigPosition.x < tunnelGoal.x)
                {
                    currentDigPosition.x++;
                }
                else if (currentDigPosition.x > tunnelGoal.x)
                {
                    currentDigPosition.x--;
                }
                else
                {
                    if (spawnRoomsAtTunnelEnds)
                    {
                        SpawnRoomTemplateAtTunnelEnd(boardGenerator, currentDigPosition);
                    }
                    
                    break;
                }

                for (int j = 0; j < tunnelWidth; j++)
                {
                    boardGenerator.WriteToBoardGrid(currentDigPosition.x, currentDigPosition.y + j, GetCharToWriteForTunnel(boardGenerator), true);

                }

            }

            for (int k = 0; k < 1000; k++)
            {
                if (currentDigPosition.y < tunnelGoal.y)
                {
                    currentDigPosition.y++;
                }
                else if (currentDigPosition.y > tunnelGoal.y)
                {
                    currentDigPosition.y--;
                }
                else
                {

                    SpawnRoomTemplateAtTunnelEnd(boardGenerator, currentDigPosition);
                    break;
                }
                for (int s = 0; s < tunnelWidth; s++)
                {
                    
                    boardGenerator.WriteToBoardGrid(currentDigPosition.x + s, currentDigPosition.y, GetCharToWriteForTunnel(boardGenerator), true);
                }
            }

        }

        char GetCharToWriteForTunnel(BoardGenerator boardGenerator)
        {
            char charToWrite;
            if (useCustomEmptySpaceCharForTunnels)
            {
                charToWrite = customEmptySpaceChar;
            }
            else
            {
                charToWrite = boardGenerator.profile.boardLibrary.GetDefaultEmptyChar();
            }
            return charToWrite;
        }

        /*
        void BuildWallsAroundTunnels(BoardGenerator boardGenerator, GridPosition currentPosition)
        {
            if (buildWallsAroundTunnels)
            {
                GridPosition northOfPos = new GridPosition(currentPosition.x, currentPosition.y + tunnelWidth);
                if (boardGenerator.TestIfSpaceIsInGridAndMatchesChar(northOfPos, boardGenerator.profile.boardLibrary.GetDefaultEmptyChar()))
                {
                    boardGenerator.WriteToBoardGrid(northOfPos.x, northOfPos.y, buildTunnelWallChar, false);
                }

                GridPosition eastOfPos = new GridPosition(currentPosition.x + tunnelWidth, currentPosition.y);
                if (boardGenerator.TestIfSpaceIsInGridAndMatchesChar(eastOfPos, boardGenerator.profile.boardLibrary.GetDefaultEmptyChar()))
                {
                    boardGenerator.WriteToBoardGrid(eastOfPos.x, eastOfPos.y, buildTunnelWallChar, false);
                }

                GridPosition southOfPos = new GridPosition(currentPosition.x, currentPosition.y - tunnelWidth);
                if (boardGenerator.TestIfSpaceIsInGridAndMatchesChar(southOfPos, boardGenerator.profile.boardLibrary.GetDefaultEmptyChar()))
                {
                    boardGenerator.WriteToBoardGrid(southOfPos.x, southOfPos.y, buildTunnelWallChar, false);
                }

                GridPosition westOfPos = new GridPosition(currentPosition.x - tunnelWidth, currentPosition.y);
                if (boardGenerator.TestIfSpaceIsInGridAndMatchesChar(westOfPos, boardGenerator.profile.boardLibrary.GetDefaultEmptyChar()))
                {
                    boardGenerator.WriteToBoardGrid(westOfPos.x, westOfPos.y, buildTunnelWallChar, false);
                }

            }
        }
        */
        

        void SpawnRoomTemplateAtTunnelEnd(BoardGenerator boardGenerator, GridPosition spawnPosition)
        {
            if (tunnelEndTemplates.Length > 0)
            {
                RoomTemplate templateToSpawn = tunnelEndTemplates[Random.Range(0, tunnelEndTemplates.Length)];
                boardGenerator.DrawTemplate(spawnPosition.x, spawnPosition.y, templateToSpawn, overWriteFilledCharacters);
            }
        }
    }
}

