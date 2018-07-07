using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : ScriptableObject
{
    public abstract void Generate(BoardGenerator boardGenerator);
}
