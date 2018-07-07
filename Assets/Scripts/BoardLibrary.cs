using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (menuName = "BoardGeneration/BoardLibrary")]
public class BoardLibrary : ScriptableObject
{
    public BoardLibraryEntry[] boardLibraryEntries;
    public BoardInstantiationTechnique instantiationTechnique;

    public Tile GetDefaultTile()
    {
        return boardLibraryEntries[0].tile;
    }

    public BoardLibraryEntry GetDefaultEntry()
    {
        return boardLibraryEntries[0];
    }


    public Dictionary<Tile, BoardLibraryEntry> BuildTileKeyLibraryDictionary()
    {
        Dictionary<Tile, BoardLibraryEntry> libraryDictionary = new Dictionary<Tile, BoardLibraryEntry>();
        for (int i = 0; i < boardLibraryEntries.Length; i++)
        {
            libraryDictionary.Add(boardLibraryEntries[i].tile, boardLibraryEntries[i]);
        }

        return libraryDictionary;
    }

    public BoardLibraryEntry CheckLibraryForTile(Tile key, Dictionary<Tile, BoardLibraryEntry> boardLibraryDictionary)
    {
        if (boardLibraryDictionary.ContainsKey(key))
        {
            return boardLibraryDictionary[key];
        }
        else
        {
            Debug.LogError("Tile key not found in BoardLibrary " + key.name);
        }

        if (key == null)
        {
            return null;
        }

        Debug.LogError("check library returned null");
        return null;
        
    }

    public Tile GetTileFromChar(char charToFind)
    {
        for (int i = 0; i < boardLibraryEntries.Length; i++)
        {
            if (boardLibraryEntries[i].characterId == charToFind)
            {
                return boardLibraryEntries[i].tile;
            }
        }

        if (charToFind == null)
        {
            return GetDefaultTile();
        }
        return null;
    }

}
