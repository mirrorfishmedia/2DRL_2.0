using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (menuName = "BoardGeneration/BoardLibrary")]
public class BoardLibrary : ScriptableObject
{
    public BoardInstantiationTechnique instantiationTechnique;

    public BoardLibraryEntry[] boardLibraryEntries;

    public void Initialize()
    {
        for (int i = 0; i < boardLibraryEntries.Length; i++)
        {
            boardLibraryEntries[i].chanceBoardLibraryEntry.BuildChanceCharListProbabilities();
        }
    }

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

    public Dictionary<char,ChanceBoardLibraryEntry> BuildChanceCharDictionary()
    {
        Dictionary<char, ChanceBoardLibraryEntry> inputCharToChanceBoardLibraryEntry = new Dictionary<char, ChanceBoardLibraryEntry>();
        for (int i = 0; i < boardLibraryEntries.Length; i++)
        {
            BoardLibraryEntry entry = boardLibraryEntries[i];

            inputCharToChanceBoardLibraryEntry.Add(entry.characterId, entry.chanceBoardLibraryEntry);

        }

        //Debug.Log("dictionary length " + inputCharToChanceBoardLibraryEntry.Count);
        return inputCharToChanceBoardLibraryEntry;

    }

    public char TestCharForChanceBeforeWritingToGrid(char charToTest)
    {
        char testedChar;
        Dictionary<char, ChanceBoardLibraryEntry> inputCharToChanceBoardLibraryEntry = BuildChanceCharDictionary();
        if (inputCharToChanceBoardLibraryEntry.ContainsKey(charToTest))
        {
            ChanceBoardLibraryEntry entry = inputCharToChanceBoardLibraryEntry[charToTest];
            if (entry.chanceChars.Length > 0)
            {
                testedChar = entry.GetChanceCharId();
            }
            else
            {
                //no change, return the original character
               // Debug.Log("No change return original");
                testedChar = charToTest;
            }
        }
        else
        {
            testedChar = '0';
        }
        

        return testedChar;
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
