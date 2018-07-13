using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    //Change the string here to change where this appears in the create menu and what it's called
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateCellularAutomata")]
    public class GeneratorCellularAutomata : Generator
    {

        //Character used for positive, filled space, corresponds to charId in BoardLibrary
        public char charForFill = 'w';
        //Character used for negative space
        public char charForEmpty = '\0';
        //Seed for random generation
        public string seed;
        //Whether to use a random seed or not
        public bool useRandomSeed;
        //Should this overwrite generators earlier than it in the sequence, by default this generator tends to do this so it should be early in the list
        private bool overwriteFilledSpaces = true;

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

            for (int x = 0; x < boardGenerator.profile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerator.profile.boardVerticalSize; y++)
                {
                    boardGenerator.WriteToBoardGrid(x, y, (pseudoRandom.Next(0, 100) < randomFillPercent) ? charForFill : charForEmpty, overwriteFilledSpaces);
                }
            }
        }

        void SmoothMap(BoardGenerator boardGenerator)
        {
            for (int x = 0; x < boardGenerator.profile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerator.profile.boardVerticalSize; y++)
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
                    if (neighbourX >= 0 && neighbourX < boardGenerator.profile.boardHorizontalSize && neighbourY >= 0 && neighbourY < boardGenerator.profile.boardVerticalSize)
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

    }

}
