﻿using System.Collections;
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
        public Tile tile;
        public TileBase tileBase;
        public ChanceBoardLibraryEntry chanceBoardLibraryEntry;
    }
}
