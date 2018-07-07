using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {

	public int roomSize = 10;
	public RoomTemplate[] roomTemplates;

	private BoardGenerator boardGenerator;

	void Awake()
	{
		boardGenerator = GetComponent<BoardGenerator> ();
    }

	public void Build(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber)
	{
        ScriptableRoom(roomOrigin, roomTemplate, chainNumber, true);
    }

    public void ScriptableRoom(Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath)
    {

        if (isOnPath)
        {
            GameObject roomHolder = new GameObject("Path Room " + chainNumber + " " + roomTemplate.name);
            roomHolder.transform.position = roomOrigin;
        }

        int charIndex = 0;
        
        for (int i = 0; i < roomSize; i++)
        {
            for (int j = 0; j < roomSize; j++)
            {
                char selectedChar = roomTemplate.roomChars[charIndex];
                if (selectedChar != '\0')
                {
                    Vector2 spawnPos = new Vector2(i, j) + roomOrigin;

                    int x = (int)spawnPos.x;
                    int y = (int)spawnPos.y;

                    boardGenerator.boardGridAsCharacters[x, y] = selectedChar;
                }
                
                charIndex++;

            }
        }
    }
}