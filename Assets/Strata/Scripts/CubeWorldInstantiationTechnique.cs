using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    [CreateAssetMenu(menuName = "BoardGeneration/CubeWorldInstantiator")]
    public class CubeWorldInstantiationTechnique : BoardInstantiationTechnique
    {
        public int mapYLayers = 3;

        public override void SpawnBoardSquare(BoardGenerator boardGenerator, Vector2 location, BoardLibraryEntry inputEntry)
        {
            if (inputEntry != null)
            {
                if (inputEntry.prefabToSpawn == null)
                {


                }
                else
                {
                    for (int i = 0; i < mapYLayers; i++)
                    {
                        Vector3 pos = new Vector3((int)location.x, i, (int)location.y);
                        SpawnCube(pos, inputEntry.prefabToSpawn);
                    }
                }
            }
            else
            {
                Debug.LogError("Returned null from library, something went wrong when trying to draw tiles.");
            }
        }

        private void SpawnCube(Vector3 spawnPosition, GameObject prefab)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

}
