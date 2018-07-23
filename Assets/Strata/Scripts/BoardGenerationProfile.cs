using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    //The board generation profile is an asset which holds all info needed to generate a level or world
    [CreateAssetMenu(menuName = "Strata/BoardGenerationProfile")]
    public class BoardGenerationProfile : ScriptableObject
    {

        //This seed allows repeatable generation of levels, or seed sharing between two players
        public string seedValue = "@mattmirrorfish";
        //The size of the board in Unity units horizontally
        public int boardHorizontalSize = 100;
        //The size of the board vertically
        public int boardVerticalSize = 100;

        //The BoardLibrary contains the mapping of tiles to characters needed to generate levels along 
        // with lists of roomtemplates used in generation of room based levels
        public BoardLibrary boardLibrary;

        //This array of generators contains all the different generator tools which will be used to generate the level
        //The generators chosen and the order that they are placed in determines the character of the generated level
        //Drag and drop different generators into this array to generate different types of levels
        public Generator[] generators;
    }

}
