using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateCellularAutomata")]
public class GeneratorCellularAutomata : Generator
{

    public char charForFill = 'w';
    public char charForEmpty = '\0';
    public string seed;
    public bool useRandomSeed;
    public bool overwriteFilledSpaces;

    [Range(0, 100)]
    public int randomFillPercent;

    public override void Generate(BoardGenerator boardGenerator)
    {
        GenerateMap(boardGenerator);
    }

    void GenerateMap(BoardGenerator boardGenerator)
    {
        RandomFillMap(boardGenerator);

        for (int i = 0; i < 5; i++)
        {
            SmoothMap(boardGenerator);
        }
    }


    void RandomFillMap(BoardGenerator boardGenerator)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < boardGenerator.boardHorizontalSize; x++)
        {
            for (int y = 0; y < boardGenerator.boardVerticalSize; y++)
            {
                boardGenerator.WriteToBoardGrid(x, y, (pseudoRandom.Next(0, 100) < randomFillPercent) ? charForFill : charForEmpty, overwriteFilledSpaces);
            }
        }
    }

    void SmoothMap(BoardGenerator boardGenerator)
    {
        for (int x = 0; x < boardGenerator.boardHorizontalSize; x++)
        {
            for (int y = 0; y < boardGenerator.boardVerticalSize; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y, boardGenerator);

                if (neighbourWallTiles > 4)
                    boardGenerator.boardGridAsCharacters[x, y] = charForFill;
                else if (neighbourWallTiles < 4)
                    boardGenerator.boardGridAsCharacters[x, y] = charForEmpty;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY, BoardGenerator boardGenerator)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < boardGenerator.boardHorizontalSize && neighbourY >= 0 && neighbourY < boardGenerator.boardVerticalSize)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        if (boardGenerator.boardGridAsCharacters[neighbourX, neighbourY] == charForFill)
                        {
                            wallCount++;
                        }
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    /*
    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
    */

}
