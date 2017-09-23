using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellCatalog : MonoBehaviour {

    public Dictionary<Tile, MapCellObject> cellDictionary = new Dictionary<Tile, MapCellObject>();

    public MapCellObject[] mapCellObjects;

	// Use this for initialization
	void Start ()
    {
        BuildCellDictionary();

    }

    public MapCellObject CheckCellCatalog(Tile key)
    {
        if (cellDictionary.ContainsKey(key))
        {
            Debug.Log("found wall key in dictionary " + key);
            return cellDictionary[key];
        }
        return null;
    }

    void BuildCellDictionary()
    {
        for (int i = 0; i < mapCellObjects.Length; i++)
        {
            if (mapCellObjects[i].tile != null)
            {
                cellDictionary.Add(mapCellObjects[i].tile, mapCellObjects[i]);
            }
            
        }
    }
    
}
