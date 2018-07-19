using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Generators/PlaceEntranceExit")]

    public class GenerateRandomContiguousItems : Generator
    {

        public char exitChar;
        public char playerChar;

        public override bool Generate(BoardGenerator boardGenerator)
        {
            PlaceExit(boardGenerator);
            PlaceStartLocation(boardGenerator);

            return true;
        }


        void PlaceExit(BoardGenerator boardGenerator)
        {
            GridPosition openConnectedPosition = boardGenerator.GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(boardGenerator);
            boardGenerator.WriteToBoardGrid(openConnectedPosition.x, openConnectedPosition.y, exitChar, true, false);
        }

        void PlaceStartLocation(BoardGenerator boardGenerator)
        {
            GridPosition openConnectedPosition = boardGenerator.GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(boardGenerator);
            boardGenerator.WriteToBoardGrid(openConnectedPosition.x, openConnectedPosition.y, playerChar, true, false);
        }


    }
}
