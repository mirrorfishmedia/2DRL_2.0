﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "BoardGeneration/Generators/GeneratorScatterRoomShapes")]
    public class GeneratorScatterShapes : Generator
    {

        public int howManyToScatter = 5;

        public RoomTemplate[] roomTemplates;
        
        public override void Generate(BoardGenerator boardGenerator)
        {
            SpawnShapes(boardGenerator);
        }

        void SpawnShapes(BoardGenerator boardGenerator)
        {
            for (int i = 0; i < howManyToScatter; i++)
            {
                RoomTemplate templateToSpawn = roomTemplates[Random.Range(0, roomTemplates.Length)];
                int randX = Random.Range(0, boardGenerator.profile.boardHorizontalSize);
                int randY = Random.Range(0, boardGenerator.profile.boardVerticalSize);
                boardGenerator.DrawTemplate(randX, randY, templateToSpawn, overwriteFilledSpaces);
            }
        }


    }
}

