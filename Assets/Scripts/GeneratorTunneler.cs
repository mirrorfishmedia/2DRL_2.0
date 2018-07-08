using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BoardGeneration/Generators/GeneratorTunneler")]
public class GeneratorTunneler : Generator
{
    public char emptySpaceChar = '0';
    public int numTunnels = 4;
    public int tunnelWidth = 1;
    public RoomTemplate[] tunnelEndTemplates;
    public bool overWriteFilledCharacters;

    public override void Generate(BoardGenerator boardGenerator)
    {
        GridPosition startPos = boardGenerator.GetRandomGridPosition();
        for (int i = 0; i < numTunnels; i++)
        {
            GridPosition randomGoalPosition = boardGenerator.GetRandomGridPosition();
            DigTunnel(boardGenerator, startPos, randomGoalPosition);
        }
    }

    public GridPosition DigTunnel(BoardGenerator boardGenerator, GridPosition startPosition, GridPosition tunnelGoal)
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
                SpawnRoomTemplateAtTunnelEnd(boardGenerator, currentDigPosition);
                break;

            }

            for (int j = 0; j < tunnelWidth; j++)
            {
                boardGenerator.WriteToBoardGrid(currentDigPosition.x, currentDigPosition.y+j, emptySpaceChar, true);
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
                boardGenerator.WriteToBoardGrid(currentDigPosition.x + s, currentDigPosition.y, emptySpaceChar, true);
            }
        }

        return currentDigPosition;
    }

    void SpawnRoomTemplateAtTunnelEnd(BoardGenerator boardGenerator, GridPosition spawnPosition)
    {
        if (tunnelEndTemplates.Length > 0)
        {
            RoomTemplate templateToSpawn = tunnelEndTemplates[Random.Range(0, tunnelEndTemplates.Length)];
            boardGenerator.DrawTemplate(spawnPosition.x, spawnPosition.y, templateToSpawn, overWriteFilledCharacters);
        }
    }
}
