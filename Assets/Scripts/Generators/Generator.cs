using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    //Abstract base class from which all generators are derived, inherit from this to create new generators with different functionality
    public abstract class Generator : ScriptableObject
    {

        public bool overwriteFilledSpaces;
        public bool generatesEmptySpace;
        public abstract void Generate(BoardGenerator boardGenerator);
    }
}
