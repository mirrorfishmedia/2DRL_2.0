using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TilemapToRoom : EditorWindow {

    public RoomTemplate roomTemplate;

    private int xSize = 10;
    private int ySize = 10;

    [MenuItem("Window/Tilemap To RoomTemplate Converter")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(TilemapToRoom)).Show();
    }

    void OnGUI()
    {

        SerializedObject serializedRoomTemplateObject = new SerializedObject(this);
        SerializedProperty serializedRoomTemplateProperty = serializedRoomTemplateObject.FindProperty("roomTemplate");
        EditorGUILayout.PropertyField(serializedRoomTemplateProperty, true);

        serializedRoomTemplateObject.ApplyModifiedProperties();

        if (GUILayout.Button("Load Room"))
        {
            ReadTilemapFromRoomTemplate();
        }

        if (GUILayout.Button("Save Room"))
        {
            WriteTilemapToRoomTemplate();
        }

        if (GUILayout.Button("Clear Selected Tilemap"))
        {
            ClearTilemap();
        }
    }

    public void ClearTilemap()
    {
        Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        tilemap.ClearAllEditorPreviewTiles();
    }

    public void WriteTilemapToRoomTemplate()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject with a Tilemap component to capture data from.");
        }
        Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
        
        int charIndex = 0;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Tile foundTile = GetTileFromGrid(x, y, tilemap);
                if (foundTile == null)
                {
                    roomTemplate.roomChars[charIndex] = '0';
                    charIndex++;
                }
                else
                {
                    MapCellObject mapCellObject;
                    mapCellObject = roomTemplate.cellCatalog.CheckCellCatalog(foundTile);
                    roomTemplate.roomChars[charIndex] = mapCellObject.symbol;
                    charIndex++;
                }

            }
        }

        Debug.Log("Success. Tilemap written to RoomTemplate");
    }

    public void ReadTilemapFromRoomTemplate()
    {
        
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("No GameObject selected. Please select a GameObject with a Tilemap component to capture data from.");
        }

        Tilemap tilemap = Selection.activeGameObject.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        int x = 0;
        int y = 0;

        Debug.Log("tilemap origin " + tilemap.origin);

        for (int i = 0; i < roomTemplate.roomChars.Length; i++)
        {
            
            if (x >= 10)
            {
                x = 0;
                y++;
            }

            Tile tileToSet = roomTemplate.cellCatalog.GetTileFromChar(roomTemplate.roomChars[i]);
            Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
            Debug.Log("x " + x + " y" + y);
            Debug.Log("pos " + pos);
            tilemap.SetTile(pos, tileToSet);

            x++;
           
        }
    }

    Tile GetTileFromGrid(int x, int y, Tilemap tilemap)
    {
        Debug.Log("X get tile " + x + " y get tile " + y);
        Vector3Int pos = new Vector3Int(x, y, 0) + tilemap.origin;
        Tile tile = tilemap.GetTile(pos) as Tile;

        return tile;
    }
}
