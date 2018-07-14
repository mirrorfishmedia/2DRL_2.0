using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{

    [CreateAssetMenu(menuName = "BoardGeneration/BoardLibrary")]
    public class BoardLibrary : ScriptableObject
    {

        public BoardInstantiationTechnique instantiationTechnique;
        public char emptySpaceCharDefault = '0';
        [Header("Room chain lists of by exit direction")]
        public RoomList movingNorthRoomTemplateList;
        public RoomList movingEastRoomTemplateList;
        public RoomList movingSouthRoomTemplateList;
        public RoomList movingWestRoomTemplateList;

        //public BoardLibraryEntry[] boardLibraryEntries;
        public List<BoardLibraryEntry> boardLibraryEntryList = new List<BoardLibraryEntry>();

        public string startingCharIdPoolForAutogeneration = "qwertyuiopasdfghjklzxcvbnm1234567890 - !@#$%^&*";

        public void Initialize()
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                boardLibraryEntryList[i].chanceBoardLibraryEntry.BuildChanceCharListProbabilities();
            }
        }

        void OnValidate()
        {
            CleanManuallyEnteredCharIdsFromAutoGenerationCharList();
        }

        public char GetDefaultEmptyChar()
        {
            return GetDefaultEntry().characterId;
        }

        public Tile GetDefaultTile()
        {
            return GetDefaultEntry().tile;
        }

        public BoardLibraryEntry GetDefaultEntry()
        {
            BoardLibraryEntry entry = null;

            //Loop over the list of entries and look for one that is flagged as useAsDefaultEmptySpace in the Inspector, return it to the calling method
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].useAsDefaultEmptySpace)
                {
                    entry = boardLibraryEntryList[i];
                    return entry;
                }
            }

            //Otherwise return entry 0 in the list, effectively a random entry if list is auto-generated
            return boardLibraryEntryList[0];
        }


        public Dictionary<Tile, BoardLibraryEntry> BuildTileKeyLibraryDictionary()
        {
            Dictionary<Tile, BoardLibraryEntry> libraryDictionary = new Dictionary<Tile, BoardLibraryEntry>();
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                libraryDictionary.Add(boardLibraryEntryList[i].tile, boardLibraryEntryList[i]);
            }

            return libraryDictionary;
        }

        public Dictionary<char, ChanceBoardLibraryEntry> BuildChanceCharDictionary()
        {
            Dictionary<char, ChanceBoardLibraryEntry> inputCharToChanceBoardLibraryEntry = new Dictionary<char, ChanceBoardLibraryEntry>();
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                BoardLibraryEntry entry = boardLibraryEntryList[i];

                inputCharToChanceBoardLibraryEntry.Add(entry.characterId, entry.chanceBoardLibraryEntry);

            }

            return inputCharToChanceBoardLibraryEntry;

        }

        public BoardLibraryEntry AddBoardLibraryEntryIfTileNotYetEntered(Tile tileToTest)
        {

            BoardLibraryEntry entry = CheckLibraryForTile(tileToTest, BuildTileKeyLibraryDictionary());
            if (entry == null)
            {
                entry = new BoardLibraryEntry();
                entry.tile = tileToTest;
                entry.characterId = RandomCharFromAllowedChars();
                boardLibraryEntryList.Add(entry);
                Debug.Log("Tile from tilemap not yet in Library. Added the tile " + entry.tile + " to " + this.name + " with charId " + entry.characterId);
            }

            return entry;
        }

        private string CleanManuallyEnteredCharIdsFromAutoGenerationCharList()
        {

            string stringWithRemovedChars = "no characters removed";
            for (int i = startingCharIdPoolForAutogeneration.Length - 1; i > 0; i--)
            {
                if (startingCharIdPoolForAutogeneration[i] == emptySpaceCharDefault)
                {
                    startingCharIdPoolForAutogeneration.Remove(i, 1);
                }

                for (int j = 0; j < boardLibraryEntryList.Count; j++)
                {
                    if (startingCharIdPoolForAutogeneration[i] == boardLibraryEntryList[j].characterId)
                    {
                        stringWithRemovedChars = startingCharIdPoolForAutogeneration.Remove(i, 1);
                        continue;
                    }
                }
            }

            return stringWithRemovedChars;
        }

        private char RandomCharFromAllowedChars()
        {
            string characterList = CleanManuallyEnteredCharIdsFromAutoGenerationCharList();
            int randomCharIndex = Random.Range(0, characterList.Length);
            char foundChar = characterList[randomCharIndex];
            characterList.Remove(randomCharIndex, 1);
            return foundChar;
        }

        private bool CheckLibraryIfCharIdExists(char charIdToTest)
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charIdToTest)
                {
                    return true;
                }
            }
            return false;
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
                return null;
            }
        }



        public Tile GetTileFromChar(char charToFind)
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charToFind)
                {
                    return boardLibraryEntryList[i].tile;
                }
            }

            if (charToFind == '\0')
            {
                return GetDefaultTile();
            }
            return null;
        }

    }

}
