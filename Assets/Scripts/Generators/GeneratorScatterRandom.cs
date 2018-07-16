using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateRandomScatter")]
    public class GeneratorScatterRandom : Generator
    {

        public char charIdToScatter = 'g';
        public int howManyToScatter = 100;

        public override void Generate(BoardGenerator boardGenerator)
        {
            Scatter(boardGenerator);
        }

        void Scatter(BoardGenerator boardGenerator)
        {
            for (int i = 0; i < howManyToScatter; i++)
            {
                int randX = Random.Range(0, boardGenerator.profile.boardHorizontalSize);
                int randY = Random.Range(0, boardGenerator.profile.boardVerticalSize);
                boardGenerator.WriteToBoardGrid(randX, randY, charIdToScatter, base.overwriteFilledSpaces, false);
            }
        }
    }
}
