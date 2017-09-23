using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapToRoom : MonoBehaviour {

    public RoomTemplate roomTemplate;

    public Tilemap tilemap;
    public int xSize = 10;
    public int ySize = 10;

    public CellCatalog cellCatalog;

    void Start()
    {
        WriteTilemapToRoomTemplate();
    }

    public void WriteTilemapToRoomTemplate()
    {
        int charIndex = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile foundTile = GetTileFromGrid(x, y);
                Debug.Log("found tile " + foundTile);
                if (foundTile == null)
                {
                    Debug.Log("found tile is null");
                    roomTemplate.roomChars[charIndex] = '0';
                    charIndex++;
                }
                else
                {
                    Debug.Log("else found tile is  " + foundTile);
                    MapCellObject mapCellObject;
                    mapCellObject = cellCatalog.CheckCellCatalog(foundTile);
                    roomTemplate.roomChars[charIndex] = mapCellObject.symbol;
                    charIndex++;
                }

            }
        } 
    }

    void DebugTileArray()
    {
        
    }

    Tile GetTileFromGrid(int x, int y)
    {
        Debug.Log("X get tile " + x + " y get tile " + y);
        Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
        Tile tile = tilemap.GetTile(pos) as Tile;

        return tile;
    }
}
