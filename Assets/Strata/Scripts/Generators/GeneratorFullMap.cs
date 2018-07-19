using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateSavedFullMap")]
    public class GeneratorFullMap : Generator
    {

        public RoomTemplate templateToSpawn;

        public override bool Generate(BoardGenerator boardGenerator)
        {
            boardGenerator.DrawTemplate(0, 0, templateToSpawn, true, false);

            return true;

        }

    }
}
