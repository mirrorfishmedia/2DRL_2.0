using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoardGeneration/Generators/GenerateSavedFullMap")]

public class GeneratorFullMap : Generator
{

    public RoomTemplate templateToSpawn;

    public override void Generate(BoardGenerator boardGenerator)
    {
        boardGenerator.DrawTemplate(0, 0, templateToSpawn, true);
    }

}
