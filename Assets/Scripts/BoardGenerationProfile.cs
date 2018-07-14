using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/BoardGenerationProfile")]
    public class BoardGenerationProfile : ScriptableObject
    {
        public string seedValue = "@mattmirrorfish";
        public int boardHorizontalSize = 100;
        public int boardVerticalSize = 100;

        public BoardLibrary boardLibrary;

        public Generator[] generators;
        public char emptySpaceChar = '0';
    }

}
