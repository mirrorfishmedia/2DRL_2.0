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

        private Dictionary<TileBase, BoardLibraryEntry> libraryDictionary;


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

            if (GUILayout.Button("Add To Enter From North List"))
            {
                FlagWithNorthAndAddToList();
            }

            if (GUILayout.Button("Add To Enter From East List"))
            {
                FlagWithEastAndAddToList();
            }

            if (GUILayout.Button("Add To Enter From South List"))
            {
                FlagWithSouthAndAddToList();
            }

            if (GUILayout.Button("Add To Enter From West List"))
            {
                FlagWithWestAndAddToList();
            }

            if (GUILayout.Button("Save Room"))
            {
                WriteTilemapToRoomTemplate();
            }

            if (roomTemplate != null)
            {
                if (GUILayout.Button("Clear & Draw Empty " + roomTemplate.roomSizeX + " x " + roomTemplate.roomSizeY))
                {
                    ClearTilemap();
                }
            }


            if (GUILayout.Button("Create New Profile & Supporting Files"))
            {
                CreateNewProfile();
            }

            if (GUILayout.Button("Create New RoomTemplate"))
            {
                CreateNewRoomTemplateAsset();
            }
        }

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();
            Debug.DrawLine(Vector3.zero, new Vector3(roomTemplate.roomSizeX, 0, 0), Color.red);
            Debug.DrawLine(Vector3.zero, new Vector3(0, roomTemplate.roomSizeY, 0), Color.red);
            Debug.DrawLine(new Vector3(roomTemplate.roomSizeX, roomTemplate.roomSizeY, 0), new Vector3(roomTemplate.roomSizeX, 0, 0), Color.red);
            Debug.DrawLine(new Vector3(roomTemplate.roomSizeX, roomTemplate.roomSizeY, 0), new Vector3(0, roomTemplate.roomSizeY, 0), Color.red);

            HandleUtility.Repaint();
            Handles.EndGUI();
        }


        public void CreateNewProfile()
        {
            BoardGenerationProfile profile = CreateAsset<BoardGenerationProfile>("New") as BoardGenerationProfile;
            
            profile.boardLibrary = CreateAsset<BoardLibrary>("New") as BoardLibrary;
            boardLibrary = profile.boardLibrary;

            profile.boardLibrary.instantiationTechnique = CreateAsset<TilemapInstantiationTechnique>("TilemapInstantiator") as TilemapInstantiationTechnique;

            profile.boardLibrary.canBeEnteredFromNorthList = CreateAsset<RoomList>("Exits North") as RoomList;
            profile.boardLibrary.canBeEnteredFromWestList = CreateAsset<RoomList>("Exits East") as RoomList;
            profile.boardLibrary.canBeEnteredFromSouthList = CreateAsset<RoomList>("Exits South") as RoomList;
            profile.boardLibrary.canBeEnteredFromEastList = CreateAsset<RoomList>("Exits West") as RoomList;
        }

        public void CreateNewRoomTemplateAsset()
        {
            roomTemplate = CreateAsset<RoomTemplate>("Room") as RoomTemplate;
        }

        public static ScriptableObject CreateAsset<T>(string assetName) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(asset, assetName + " " + typeof(T).Name + ".asset");
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
                    TileBase foundTile = GetTileFromGrid(x, y, tilemap) as TileBase;
                    //RuleTile foundRuleTile = GetTileFromGrid(x, y, tilemap);
                    
                    //Debug.Log("foundRuleTile " + foundRuleTile);
                    if (foundTile == null)
                    {
                        //If tilemap is blank inside grid, write in default empty space character defined in board library, usually 0
                        roomTemplate.roomChars[charIndex] = boardLibrary.GetDefaultEmptyChar();
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
            roomTemplate.opensToNorth = true;
            boardLibrary.canBeEnteredFromSouthList.RemoveEmptyEntriesThenAdd(roomTemplate);
        }

        public void FlagWithEastAndAddToList()
        {
            roomTemplate.opensToEast = true;
            boardLibrary.canBeEnteredFromWestList.RemoveEmptyEntriesThenAdd(roomTemplate);
        }

        public void FlagWithSouthAndAddToList()
        {
            roomTemplate.opensToSouth = true;
            boardLibrary.canBeEnteredFromNorthList.RemoveEmptyEntriesThenAdd(roomTemplate);
        }

        public void FlagWithWestAndAddToList()
        {
            roomTemplate.opensToWest = true;
            boardLibrary.canBeEnteredFromEastList.RemoveEmptyEntriesThenAdd(roomTemplate);
        }

        public void LoadTileMapFromRoomTemplate()
        {
            Debug.Log("load");
            libraryDictionary = boardLibrary.BuildTileKeyLibraryDictionary();
            Tilemap tilemap = SelectTilemapInScene();

            tilemap.ClearAllTiles();
            Debug.Log("clear");
            int charIndex = 0;
            for (int x = 0; x < roomTemplate.roomSizeX; x++)
            {
                for (int y = 0; y < roomTemplate.roomSizeY; y++)
                {
                    TileBase tileToSet = boardLibrary.GetTileFromChar(roomTemplate.roomChars[charIndex]);
                    Debug.Log("tile to set " + tileToSet);
                    if (tileToSet == null)
                    {
                        Debug.LogError("Attempting to load empty tiles, draw and save something first");
                        
                    }

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

        TileBase GetTileFromGrid(int x, int y, Tilemap tilemap)
        {
            Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
            TileBase tile = tilemap.GetTile(pos);
            //Debug.Log("get tile " + tilemap.GetTile(pos));
            return tile;
        }
    }
}

