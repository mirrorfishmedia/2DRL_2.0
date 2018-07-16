using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Strata
{
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

