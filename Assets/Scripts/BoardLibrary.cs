using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "BoardGeneration/BoardLibrary")]
public class BoardLibrary : ScriptableObject
{
    public BoardLibraryEntry[] boardLibraryEntries;
    public BoardInstantiationTechnique instantiationTechnique;

    
}
