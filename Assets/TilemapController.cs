using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour {

	private Tilemap tileMap;

	public Tile testTile1;
	public Tile testTile2;

	void Awake()
	{
		tileMap = GetComponent<Tilemap> ();
	}

	// Use this for initialization
	void Start () 
	{
		tileMap.ClearAllTiles ();
		tileMap.SetTile (new Vector3Int (10, 10, 0), testTile1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
