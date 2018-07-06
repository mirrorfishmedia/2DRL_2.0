using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardinalDirection
{
    public enum Direction { North, East, South, West};
	
    public Vector2 GetVectorFromDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return new Vector2(1, 0);
            case Direction.East:
                 return new Vector2(0, 1);
            case Direction.South:
                return new Vector2(-1, 0);
            case Direction.West:
                return new Vector2(0, -1);
            default:
                break;
        }

        return Vector2.zero;
    }

    public Vector2 GetRandomVector2Cardinal()
    {
        int randomChoice = Random.Range(0, 4);

        switch (randomChoice)
        {
            case 0:
                return new Vector2(1, 0);
            case 1:
                return new Vector2(0, 1);
            case 2:
                return new Vector2(-1, 0);
            case 3:
                return new Vector2(0, -1);
            default:
                break;
        }

        return Vector2.zero;
    }

    public Vector2 GetRandomCardinalDirExcluding(Vector2 excludedVector2)
    {
        
        List<Vector2> dirList = new List<Vector2>();
        dirList.Add(new Vector2(1, 0));
        dirList.Add(new Vector2(0, 1));
        dirList.Add(new Vector2(-1, 0));
        dirList.Add(new Vector2(0, -1));

        dirList.Remove(excludedVector2);
        int randomListChoice = Random.Range(0, 3);
        
        return dirList[randomListChoice];
    }
}
