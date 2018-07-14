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
            //Look through the BoardLibrary to see if there is already a matching tile entered
            BoardLibraryEntry entry = CheckLibraryForTile(tileToTest, BuildTileKeyLibraryDictionary());
            if (entry == null)
            {
                //If there is no Tile entered, create a new entry
                entry = new BoardLibraryEntry();

                //And set it's tile to the tile we were testing
                entry.tile = tileToTest;
                //Get the first character of the name of the tile we're using to assign that as it's new charId
                char firstCharInTileName = tileToTest.name[0];

                //Check existing entries to see if this charId is already used
                if (!CheckBoardLibraryForUsedCharIds(firstCharInTileName))
                {
                    //If not, assign the first letter of the name as the charId
                    entry.characterId = firstCharInTileName;
                }
                else
                {
                    //If it is already used, then assign a random character from the string of allowed characters defined in startingCharIdPoolForAutogeneration
                    entry.characterId = RandomCharFromAllowedChars();
                }
                
                //Add the new entry
                boardLibraryEntryList.Add(entry);
                Debug.Log("Tile from tilemap not yet in Library. Added the tile " + entry.tile + " to " + this.name + " with charId " + entry.characterId);
            }

            return entry;
        }

        public bool CheckBoardLibraryForUsedCharIds(char charToTest)
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charToTest)
                {
                    return true;
                }
            }

            return false;
        }

        private char RandomCharFromAllowedChars()
        {
            string characterString = CleanManuallyEnteredCharIdsFromAutoGenerationCharList();
            int randomCharIndex = Random.Range(0, characterString.Length);
            char foundChar = characterString[randomCharIndex];
            characterString.Remove(randomCharIndex, 1);
            return foundChar;
        }

        private string CleanManuallyEnteredCharIdsFromAutoGenerationCharList()
        {

            string stringWithRemovedChars = "no characters removed yet";
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
