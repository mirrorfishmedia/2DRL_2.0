using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Strata
{

    //Simple Custom Editor for BoardGenerator that adds the button needed to generate in the scene view without entering playmode
    [CustomEditor(typeof(BoardGenerator))]
    public class BoardGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BoardGenerator myScript = (BoardGenerator)target;
            if (GUILayout.Button("Clear And Generate"))
            {
                myScript.ClearAndRebuild();
            }

            if (GUILayout.Button("Clear"))
            {
                myScript.ClearLevel();
            }
        }
    }

}

