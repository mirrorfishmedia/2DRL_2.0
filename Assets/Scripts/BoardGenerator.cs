using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{

    public class BoardGenerator : MonoBehaviour
    {

        public bool buildOnStart;
        public bool randomSeed;
        public bool useDailySeed;
        public Tilemap tilemap;
        public BoardGenerationProfile profile;


        [HideInInspector]
        public List<Vector2> exitLocations;

        public Vector2 lastChainDirectionMoved;

        [HideInInspector]
        public List<Vector2> roomChainRoomLocationsFilled = new List<Vector2>();
        public List<RoomAndDirection> branchDirections = new List<RoomAndDirection>();
        public List<Vector2> roomChainPathBranchLocations = new List<Vector2>();

        //public List<List<GridPosition>> emptySpaceListsFromGenerators = new List<List<GridPosition>>();
        //public List<GridPosition>[] emptySpacesGeneratedLists;

        public List<GridPositionList> emptySpaceLists = new List<GridPositionList>();


        public int currentGeneratorIndexIdForEmptySpaceTracking = 0;

        [HideInInspector]
        public Vector2 currentLocation;
        [HideInInspector]
        public int roomsOnPathCreated;
        [HideInInspector]
        public RoomTemplate currentChainRoom;

        public char[,] boardGridAsCharacters;
        private Dictionary<char, BoardLibraryEntry> libraryDictionary;


        // Use this for initialization
        void Start()
        {
           
            if (buildOnStart)
            {
                BuildLevel();
            }
        }

        void SetRandomStateFromStringSeed()
        {
            int seedInt = 0;
            if (randomSeed)
            {
                seedInt = Random.Range(0, 100000);
            }
            else if (useDailySeed)
            {
                seedInt = System.DateTime.Today.GetHashCode();
            }
            else
            {
                seedInt = profile.seedValue.GetHashCode();

            }

            Debug.Log("seedInt = " + seedInt);
            Random.InitState(seedInt);

        }

        void BuildLevel()
        {
            SetRandomStateFromStringSeed();
            profile.boardLibrary.Initialize();
            InitializeLibraryDictionary();

            if (tilemap != null)
            {
                tilemap.ClearAllTiles();
            }
            SetupEmptyGrid();
            RunGenerators();
            InstantiateGeneratedLevelData();
        }


#if UNITY_EDITOR
        //Checking to see if the 0 (zero) key is pressed during play mode, only in the Unity Editor. Remove the if/endif if you want this in your build for testing.
        private void Update()
        {
            //Check for the 0 key
            if (Input.GetKeyUp(KeyCode.Keypad0))
            {
                //And empty all collections and data, then rebuild the level.
                ClearAndRebuild();
            }
        }

        public void ClearAndRebuild()
        {
            //if(boardGridAsCharacters.Length)
            ClearLevel();
            BuildLevel();
        }
#endif
        //Clear out all local variables and regenerate the level, useful for testing your algorithms quickly, enter play mode and press 0 repeatedly
        //Worth noting that this does allocate significant memory so you probably don't want to be repeatedly generating levels during performance critical gameplay.
        public void ClearLevel()
        {
            tilemap.ClearAllTiles();
            roomChainRoomLocationsFilled.Clear();
            currentLocation = Vector2.zero;
            roomsOnPathCreated = 0;
            currentChainRoom = null;
            emptySpaceLists.Clear();
            currentGeneratorIndexIdForEmptySpaceTracking = 0;
            branchDirections.Clear();
            roomChainPathBranchLocations.Clear();
            SetupEmptyGrid();

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }

        }


        void SetupEmptyGrid()
        {
            boardGridAsCharacters = new char[profile.boardHorizontalSize, profile.boardVerticalSize];
            for (int i = 0; i < profile.boardHorizontalSize; i++)
            {
                for (int j = 0; j < profile.boardVerticalSize; j++)
                {
                    boardGridAsCharacters[i, j] = profile.boardLibrary.GetDefaultEmptyChar();
                }
            }
        }

        public void InitializeLibraryDictionary()
        {
            libraryDictionary = new Dictionary<char, BoardLibraryEntry>();
            for (int i = 0; i < profile.boardLibrary.boardLibraryEntryList.Count; i++)
            {
                libraryDictionary.Add(profile.boardLibrary.boardLibraryEntryList[i].characterId, profile.boardLibrary.boardLibraryEntryList[i]);
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
                if (charId == '\0')
                {
                    return profile.boardLibrary.GetDefaultEntry();
                }
                
            }

            return entry;
        }

        void RunGenerators()
        {
            for (int i = 0; i < profile.generators.Length; i++)
            {
                emptySpaceLists.Add(new GridPositionList());
                if (profile.generators[i].generatesEmptySpace)
                {
                    currentGeneratorIndexIdForEmptySpaceTracking = i;
                }
                
                profile.generators[i].Generate(this);
            }
        }

        void InstantiateGeneratedLevelData()
        {
            for (int x = 0; x < profile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < profile.boardVerticalSize; y++)
                {
                    Vector2 spawnPos = new Vector2(x, y);
                    CreateMapEntryFromGrid(boardGridAsCharacters[x, y], spawnPos);
                }
            }
        }

        public void CreateMapEntryFromGrid(char charId, Vector2 position)
        {
            BoardLibraryEntry entryToSpawn = GetLibraryEntryViaCharacterId(charId);
            if (entryToSpawn != null)
            {
                profile.boardLibrary.instantiationTechnique.SpawnBoardSquare(this, position, entryToSpawn);
            }
        }

        bool RoomChainSpaceValid(Vector2 spaceToTest)
        {
            if (roomChainRoomLocationsFilled.Contains(spaceToTest))
            {
                Debug.Log("space filled");
                return false;
            }
            else
            {
                Debug.Log("space empty");
                if (spaceToTest.x < profile.boardHorizontalSize && spaceToTest.y < profile.boardVerticalSize && spaceToTest.x > 0 && spaceToTest.y > 0)
                {
                    Debug.Log("space valid in board, roomsOnPathCreated: " + roomsOnPathCreated);
                    return true;
                }
                else
                {
                    Debug.Log("reached board edge: " + roomsOnPathCreated);
                    return false;
                }
            }
        }

        public GridPosition GetRandomGridPosition()
        {
            GridPosition randomPosition = new GridPosition(Random.Range(0, profile.boardHorizontalSize), Random.Range(0, profile.boardVerticalSize));
            return randomPosition;
        }

        public void DrawTemplate(int x, int y, RoomTemplate templateToSpawn, bool overWriteFilledCharacters, bool inConnectedPlayableArea)
        {
            int charIndex = 0;
            for (int i = 0; i < templateToSpawn.roomSizeX; i++)
            {
                for (int j = 0; j < templateToSpawn.roomSizeY; j++)
                {
                    WriteToBoardGrid(x + i, y + j, templateToSpawn.roomChars[charIndex], overWriteFilledCharacters, inConnectedPlayableArea);
                    charIndex++;
                }
            }
        }


        bool TestIfInGrid(int x, int y)
        {
            if (x < profile.boardHorizontalSize && y < profile.boardVerticalSize && x >= 0 && y >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TestIfSpaceIsInGridAndMatchesChar(GridPosition spaceToTest, char charToTest)
        {
            if (TestIfInGrid(spaceToTest.x, spaceToTest.y))
            {
                if (boardGridAsCharacters[spaceToTest.x, spaceToTest.y] == charToTest)
                {
                    return true;
                }
            }

            return false;
        }

        public void WriteToBoardGrid(int x, int y, char charIdToWrite, bool overwriteFilledSpaces, bool inConnectedPlayableArea)
        {
            if (TestIfInGrid(x, y))
            {
                if (overwriteFilledSpaces)
                {
                    char nextChar = profile.boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                    boardGridAsCharacters[x, y] = nextChar;
                }
                else
                {
                    if (boardGridAsCharacters[x, y] == profile.boardLibrary.GetDefaultEmptyChar())
                    {
                        char nextChar = profile.boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                        boardGridAsCharacters[x, y] = nextChar;
                    }
                }

                if (boardGridAsCharacters[x, y] == profile.boardLibrary.GetDefaultEmptyChar() && inConnectedPlayableArea)
                {
                    //Wrote an empty space to grid, let's add it to our list of lists
                    GridPosition emptyPosition = new GridPosition(x, y);
                    RecordEmptySpacesLeftByEachGenerator(emptyPosition);
                }
                else
                {
                    GridPosition filledPosition = new GridPosition(x, y);
                    RemoveFilledSpaceFromEmptyLists(filledPosition);
                }
            }

        }

        public void RecordEmptySpacesLeftByEachGenerator(GridPosition emptyPosition)
        {
            emptySpaceLists[currentGeneratorIndexIdForEmptySpaceTracking].gridPositionList.Add(emptyPosition);
        }

        public void RemoveFilledSpaceFromEmptyLists(GridPosition filledPosition)
        {
            for (int i = 0; i < emptySpaceLists.Count; i++)
            {
                for (int j = emptySpaceLists[i].gridPositionList.Count - 1; j > -1; j--)
                {
                    if (emptySpaceLists[i].gridPositionList[j].x == filledPosition.x && emptySpaceLists[i].gridPositionList[j].y == filledPosition.y)
                    {
                        emptySpaceLists[i].gridPositionList.RemoveAt(j);
                    }

                }
            }

        }





        public GridPosition GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(BoardGenerator boardGenerator)
        {
            int genIndex = 0;

            for (int i = 0; i < profile.generators.Length; i++)
            {
                if (profile.generators[i].generatesEmptySpace)
                {
                    genIndex = i;
                }
            }

            GridPosition randPosition = emptySpaceLists[genIndex].gridPositionList[Random.Range(0, emptySpaceLists[genIndex].gridPositionList.Count)];

            return randPosition;
        }
    }
}

