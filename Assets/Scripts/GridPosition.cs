using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    public class GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }

        public GridPosition Vector2ToGridPosition(Vector2 vectorPos)
        {
            GridPosition convertedPosition = new GridPosition((int)vectorPos.x, (int)vectorPos.y);

            return convertedPosition;
        }
    }
}
