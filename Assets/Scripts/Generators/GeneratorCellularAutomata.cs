using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is adapted from Sebastian Lague's procedural cave generation tutorial, for more information on cellular automata and the technique applied to 
 * marching squares 3d mesh generation please check out his series: https://unity3d.com/learn/tutorials/s/procedural-cave-generation-tutorial
 */


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
        //Whether to use a random seed or not
        public bool useRandomSeed;
        //Should this overwrite generators earlier than it in the sequence, by default this generator tends to do this so it should be early in the list
        private bool overwriteFilledSpaces = true;

        //How much to fill space in the map, try values from 40-55
        [Range(0, 100)]
        public int randomFillPercent;

        //This is the function that will be called by BoardGenerator to kick off the generation process
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
            for (int x = 0; x < boardGenerator.profile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerator.profile.boardVerticalSize; y++)
                {
                    boardGenerator.WriteToBoardGrid(x, y, (Random.Range(0,100) < randomFillPercent) ? charForFill : charForEmpty, overwriteFilledSpaces);
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
                        boardGenerator.WriteToBoardGrid(x,y,charForFill,overwriteFilledSpaces);
                    else if (neighbourWallTiles < 4)
                        boardGenerator.WriteToBoardGrid(x, y, charForEmpty, overwriteFilledSpaces);

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
