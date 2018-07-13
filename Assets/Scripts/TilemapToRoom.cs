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
        }

        public void ClearTilemap()
        {
            SelectTilemapInScene();
            Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
            tilemap.ClearAllTiles();
            tilemap.ClearAllEditorPreviewTiles();
            WriteTemplateSquare();
        }

        public void WriteTemplateSquare()
        {
            Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
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

            SelectTilemapInScene();
            Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();

            int charIndex = 0;
            for (int x = 0; x < roomTemplate.roomSizeX; x++)
            {
                for (int y = 0; y < roomTemplate.roomSizeY; y++)
                {
                    Tile foundTile = GetTileFromGrid(x, y, tilemap);
                    if (foundTile == null)
                    {
                        roomTemplate.roomChars[charIndex] = '0';
                        charIndex++;
                    }
                    else
                    {
                        BoardLibraryEntry entry;
                        entry = boardLibrary.CheckLibraryForTile(foundTile, libraryDictionary);

                        if (entry == null)
                        {
                            Debug.LogError("Tile not found: " + entry.tile + " Have you added it to your BoardLibrary yet?");
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

            SelectTilemapInScene();
            Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
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

        public void SelectTilemapInScene()
        {

            libraryDictionary = boardLibrary.BuildTileKeyLibraryDictionary();

            if (Selection.activeGameObject == null)
            {
                Selection.activeGameObject = FindObjectOfType<Tilemap>().gameObject;

            }
            else if (Selection.activeGameObject.GetComponent<Tilemap>() == null)
            {
                Debug.LogError("No Tilemap on GameObject selected. Please select a GameObject with a Tilemap component to capture data from.");
            }
        }

        Tile GetTileFromGrid(int x, int y, Tilemap tilemap)
        {
            Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
            Tile tile = tilemap.GetTile(pos) as Tile;

            return tile;
        }
    }
}

