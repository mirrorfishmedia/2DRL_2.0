using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    //Simple little helper class to store and work with X,Y positions as ints
    [System.Serializable]
    public class GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }


        //Use this to input a Vector2 and return a GridPosition
        public GridPosition Vector2ToGridPosition(Vector2 vectorPos)
        {
            GridPosition convertedPosition = new GridPosition((int)vectorPos.x, (int)vectorPos.y);

            return convertedPosition;
        }

        //Convert a GridPosition to a Vector2
        public Vector2 GridPositionToVector2(GridPosition gridPosition)
        {
            Vector2 convertedPosition = new Vector2(gridPosition.x, gridPosition.y);
            return convertedPosition;
        }
    }
}
