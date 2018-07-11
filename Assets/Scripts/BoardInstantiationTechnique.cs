using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardInstantiationTechnique : ScriptableObject
{
    public abstract void SpawnBoardSquare(BoardGenerator boardGenerator, Vector2 location, BoardLibraryEntry inputEntry);
}
