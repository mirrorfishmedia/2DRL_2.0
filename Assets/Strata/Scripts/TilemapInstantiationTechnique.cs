using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{
    //This is the main InstantiationTechnique for Strata, it matches an array of ASCII characters to TileBase objects and draws them on a Tilemap
    [CreateAssetMenu(menuName = "Strata/TileMapInstantiator")]
    public class TilemapInstantiationTechnique : BoardInstantiationTechnique
    {
        public override void SpawnBoardSquare(BoardGenerator boardGenerator, Vector2 location, BoardLibraryEntry inputEntry)
        {
            if (inputEntry != null)
            {
                if (inputEntry.prefabToSpawn == null)
                {
                    
                    Vector3Int pos = new Vector3Int((int)location.x, (int)location.y, 0);
                    //Write the Tile in the BoardLibraryEntry to the Tilemap
                    boardGenerator.tilemap.SetTile(pos, inputEntry.tile);
                }
                else
                {
                    //If there is a prefab to spawn, spawn that instead of setting a tile
                    Instantiate(inputEntry.prefabToSpawn, location, Quaternion.identity);
                }

            }
            else
            {
                Debug.LogError("Returned null from library, something went wrong when trying to draw tiles.");
            }
        }
    }

}
