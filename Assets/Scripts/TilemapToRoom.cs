using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Strata
{
    public class TilemapToRoom : EditorWindow
    {

        public RoomTemplate roomTemplate;
        public BoardLibrary boardLibrary;

        private Dictionary<Tile, BoardLibraryEntry> libraryDictionary;

        private List<char> charsAlreadyUsedInBoardLibrary = new List<char>();

        [MenuItem("Window/Tilemap To RoomTemplate Converter")]
        static void Init()
        {
            EditorWindow.GetWindow(typeof(TilemapToRoom)).Show();
        }

        void OnGUI()
        {

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedRoomTemplateProperty = serializedObject.FindProperty("roomTemplate");
            EditorGUILayout.PropertyField(serializedRoomTemplateProperty, true);

            SerializedProperty serializedBoardLibraryProperty = serializedObject.FindProperty("boardLibrary");
            EditorGUILayout.PropertyField(serializedBoardLibraryProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Load Room"))
            {
                LoadTileMapFromRoomTemplate();
            }

            if (GUILayout.Button("Add To North Exiting Rooms List"))
            {
                FlagWithNorthAndAddToList();
            }

            if (GUILayout.Button("Add To East Exiting Rooms List"))
            {
                FlagWithEastAndAddToList();
            }

            if (GUILayout.Button("Add To South Exiting Rooms List"))
            {
                FlagWithSouthAndAddToList();
            }

            if (GUILayout.Button("Add To West Exiting Rooms List"))
            {
                FlagWithWestAndAddToList();
            }

            if (GUILayout.Button("Save Room"))
            {
                WriteTilemapToRoomTemplate();
            }

            if (GUILayout.Button("Clear & Draw Empty " + roomTemplate.roomSizeX + " x " + roomTemplate.roomSizeY))
            {
                ClearTilemap();
            }

            if (GUILayout.Button("Create New  BoardLibrary"))
            {
                CreateNewBoardLibraryAsset();
            }
        }

        public void CreateNewBoardLibraryAsset()
        {
            boardLibrary = CreateAsset<BoardLibrary>("Library") as BoardLibrary;
            boardLibrary.movingNorthRoomTemplateList = CreateAsset<RoomList>(boardLibrary.name + " Moving North") as RoomList;
            boardLibrary.movingEastRoomTemplateList = CreateAsset<RoomList>(boardLibrary.name + " Moving East") as RoomList;
            boardLibrary.movingSouthRoomTemplateList = CreateAsset<RoomList>(boardLibrary.name + " Moving South") as RoomList;
            boardLibrary.movingWestRoomTemplateList = CreateAsset<RoomList>(boardLibrary.name + " Moving West") as RoomList;
        }

        void ReadTilesFromPalette()
        {
            
        }

        public static ScriptableObject CreateAsset<T>(string assetName) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, "New " + assetName + " " + typeof(T).Name + ".asset");
            return asset;
        }

        public void ClearTilemap()
        {
            Tilemap tilemap = SelectTilemapInScene();
            tilemap.ClearAllTiles();
            tilemap.ClearAllEditorPreviewTiles();
            WriteTemplateSquare();
        }

        public void WriteTemplateSquare()
        {
            Tilemap tilemap = SelectTilemapInScene();
            Vector3Int origin = tilemap.origin;

            for (int x = 0; x < roomTemplate.roomSizeX; x++)
            {
                for (int y = 0; y < roomTemplate.roomSizeY; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0) + origin;
                    tilemap.SetTile(tilePos, boardLibrary.GetDefaultEntry().tile);
                }
            }
        }

        public void WriteTilemapToRoomTemplate()
        {
            libraryDictionary = boardLibrary.BuildTileKeyLibraryDictionary();

            Tilemap tilemap = SelectTilemapInScene();
            int charIndex = 0;
            for (int x = 0; x < roomTemplate.roomSizeX; x++)
            {
                for (int y = 0; y < roomTemplate.roomSizeY; y++)
                {
                    Tile foundTile = GetTileFromGrid(x, y, tilemap);
                    if (foundTile == null)
                    {
                        //If tilemap is blank inside grid, write in default empty space character defined in board library, usually 0
                        roomTemplate.roomChars[charIndex] = boardLibrary.emptySpaceCharDefault;
                        charIndex++;
                    }
                    else
                    {
                        BoardLibraryEntry entry;
                        entry = boardLibrary.CheckLibraryForTile(foundTile, libraryDictionary);

                        if (entry == null)
                        {
                            entry = boardLibrary.AddBoardLibraryEntryIfTileNotYetEntered(foundTile);


                        }
                        roomTemplate.roomChars[charIndex] = entry.characterId;
                        charIndex++;
                    }

                }
            }

            Debug.Log("Success. Tilemap written to RoomTemplate");
        }


        public void FlagWithNorthAndAddToList()
        {
            roomTemplate.hasNorthExit = true;
            boardLibrary.movingNorthRoomTemplateList.roomList.Add(roomTemplate);
        }

        public void FlagWithEastAndAddToList()
        {
            roomTemplate.hasEastExit = true;
            boardLibrary.movingEastRoomTemplateList.roomList.Add(roomTemplate);
        }

        public void FlagWithSouthAndAddToList()
        {
            roomTemplate.hasSouthExit = true;
            boardLibrary.movingSouthRoomTemplateList.roomList.Add(roomTemplate);
        }

        public void FlagWithWestAndAddToList()
        {
            roomTemplate.hasWestExit = true;
            boardLibrary.movingWestRoomTemplateList.roomList.Add(roomTemplate);
        }

        public void LoadTileMapFromRoomTemplate()
        {
            libraryDictionary = boardLibrary.BuildTileKeyLibraryDictionary();
            Tilemap tilemap = SelectTilemapInScene();

            tilemap.ClearAllTiles();

            int charIndex = 0;
            for (int x = 0; x < roomTemplate.roomSizeX; x++)
            {
                for (int y = 0; y < roomTemplate.roomSizeY; y++)
                {
                    Tile tileToSet = boardLibrary.GetTileFromChar(roomTemplate.roomChars[charIndex]);
                    Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
                    tilemap.SetTile(pos, tileToSet);
                    charIndex++;

                }
            }
        }

        public Tilemap SelectTilemapInScene()
        {

         

            if (Selection.activeGameObject == null)
            {
                return FindTileMapInScene();
            }
            else
            {
                Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();

                if (tilemap != null)
                {
                    return tilemap;
                }
                else
                {
                    return FindTileMapInScene();
                }
            }
        }

        Tilemap FindTileMapInScene()
        {
            Tilemap tilemap = FindObjectOfType<Tilemap>();
            if (tilemap == null)
            {
                tilemap = AddTilemapToScene();
                Selection.activeGameObject = tilemap.gameObject;
                return tilemap;
            }

            Selection.activeGameObject = tilemap.gameObject;
            return tilemap;
        }


        Tilemap AddTilemapToScene()
        {
            Debug.Log("No Tilemap found in scene. Creating a new one.");
            GameObject grid = new GameObject("Strata Grid");
            GameObject tilemapGameObject = new GameObject("Strata Tilemap");
            tilemapGameObject.transform.SetParent(grid.transform);
            grid.AddComponent<Grid>();
            Selection.activeGameObject = grid;
            Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();
            tilemapGameObject.AddComponent<TilemapRenderer>();

            return tilemap;
        }

        Tile GetTileFromGrid(int x, int y, Tilemap tilemap)
        {
            Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
            Tile tile = tilemap.GetTile(pos) as Tile;

            return tile;
        }
    }
}

