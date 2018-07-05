using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardInstantiator : MonoBehaviour {

    private Dictionary<char, BoardLibraryEntry> libraryDictionary;
    public BoardLibrary boardLibrary;
    public Tilemap tilemap;

    public void Initialize()
    {
        libraryDictionary = new Dictionary<char, BoardLibraryEntry>();
        for (int i = 0; i < boardLibrary.boardLibraryEntries.Length; i++)
        {
            libraryDictionary.Add(boardLibrary.boardLibraryEntries[i].characterId, boardLibrary.boardLibraryEntries[i]);
            Debug.Log("library dictionary count " + libraryDictionary.Count);
        }
    }

    public BoardLibraryEntry GetLibraryEntryViaCharacterId(char charId)
    {
        BoardLibraryEntry entry = null;
        if (libraryDictionary.ContainsKey(charId))
        {
            entry = libraryDictionary[charId];
        }
        else
        {
            Debug.LogError("Attempt to get charId " + charId.ToString() + " from boardLibrary failed, is there an entry with that ID in the boardLibrary?");
        }

        return entry;
    }

    public void InstantiateTile(Vector2 posToSpawn, char charId)
    {
        BoardLibraryEntry entryToSpawn = GetLibraryEntryViaCharacterId(charId);
        boardLibrary.instantiationTechnique.SpawnBoardSquare(posToSpawn, entryToSpawn, this);
    }

}
