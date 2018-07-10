using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class BoardLibraryEntry
{
    public char characterId;
    public GameObject prefabToSpawn;
    public Tile tile;
    public float traversable = 1f;
    public ChanceBoardLibraryEntry chanceBoardLibraryEntry;
}
