using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorSpaceList
{
    public Generator generatorType;
    public char charToRecord = '\0';
    public List<GridPosition> positionList;

    public void Initialize()
    {
        positionList = new List<GridPosition>();
    }

    public GridPosition GetRandomRecordedPositionFromGenerator()
    {
        Debug.Log("positionList.Count " + positionList.Count);
        int randomIndex = Random.Range(0, positionList.Count);
        Debug.Log("randomIndex " + randomIndex);
        GridPosition randomPosition = positionList[randomIndex];
        return randomPosition;
    }
}
