using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{
    [System.Serializable]
    public class BoardLibraryEntry
    {
        public char characterId;
        public bool useAsDefaultEmptySpace;
        public GameObject prefabToSpawn;
        public TileBase tile;
        public ChanceBoardLibraryEntry chanceBoardLibraryEntry;
    }
}
