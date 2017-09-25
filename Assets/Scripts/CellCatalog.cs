using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2DRL/CellCatalog")]
public class CellCatalog : ScriptableObject {

    public Dictionary<Tile, MapCellObject> cellDictionary = new Dictionary<Tile, MapCellObject>();

    public MapCellObject[] mapCellObjects;

    public MapCellObject CheckCellCatalog(Tile key)
    {
        if (key == null)
            return null;
        BuildCellDictionary();
        if (cellDictionary.ContainsKey(key))
        {
            return cellDictionary[key];
        }
        return null;
    }

    public Tile GetTileFromChar(char charToFind)
    {
        for (int i = 0; i < mapCellObjects.Length; i++)
        {
            if (mapCellObjects[i].symbol == charToFind)
            {
                return mapCellObjects[i].tile;
            }
        }
        return null;
    }

    void BuildCellDictionary()
    {
        for (int i = 0; i < mapCellObjects.Length; i++)
        {
            if (mapCellObjects[i].tile != null)
            {
                if (!cellDictionary.ContainsKey(mapCellObjects[i].tile))
                {
                    cellDictionary.Add(mapCellObjects[i].tile, mapCellObjects[i]);
                }
                
            }
        }
    }
}
