using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/PlaceEntranceExit")]

    public class GenerateRandomContiguousItems : Generator
    {

        public char exitChar;
        public char playerChar;

        public override void Generate(BoardGenerator boardGenerator)
        {
            PlaceExit(boardGenerator);
            PlaceStartLocation(boardGenerator);
        }


        void PlaceExit(BoardGenerator boardGenerator)
        {
            GridPosition openConnectedPosition = boardGenerator.GetRandomEmptyGridPositionByGeneratorIndex(boardGenerator.currentGeneratorIndexIdForEmptySpaceTracking - 1);
            boardGenerator.WriteToBoardGrid(openConnectedPosition.x, openConnectedPosition.y, exitChar, true);
        }

        void PlaceStartLocation(BoardGenerator boardGenerator)
        {
            GridPosition openConnectedPosition = boardGenerator.GetRandomEmptyGridPositionByGeneratorIndex(boardGenerator.currentGeneratorIndexIdForEmptySpaceTracking - 1);
            boardGenerator.WriteToBoardGrid(openConnectedPosition.x, openConnectedPosition.y, playerChar, true);
        }


    }
}
