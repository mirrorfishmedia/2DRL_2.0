﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "BoardGeneration/TileMapInstantiator")]
public class TilemapInstantiationTechnique : BoardInstantiationTechnique
{
    public override void SpawnBoardSquare(Vector2 location, BoardLibraryEntry inputEntry, BoardInstantiator boardInstantiator)
    {
        if (inputEntry != null)
        {
            if (inputEntry.prefabToSpawn == null)
            {
                Vector3Int pos = new Vector3Int((int)location.x, (int)location.y, 0);
                boardInstantiator.tilemap.SetTile(pos, inputEntry.tile);
            }
            else
            {
                Instantiate(inputEntry.prefabToSpawn, location, Quaternion.identity);
            }
            
        }
        else
        {
            Debug.LogError("Returned null from library, something went wrong when trying to draw tiles.");
        }
    }
}
