using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardInstantiationTechnique : ScriptableObject
{
    public abstract void SpawnBoardSquare(Vector2 location, BoardLibraryEntry inputEntry, BoardInstantiator boardInstantiator);
}
