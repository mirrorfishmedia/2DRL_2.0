using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Strata
{
    public class BSPLeaf
    {
        public GridPosition origin;
        public int width;
        public int height;
        public BSPLeaf childA;
        public BSPLeaf childB;
        public int splitHorizontalChance = 50;
        public int minLeafSize = 6;

        public BSPLeaf(GridPosition inputOrigin, int inputWidth, int inputHeight)
        {
            origin = inputOrigin;
            width = inputWidth;
            height = inputHeight;
        }

        public bool Split()
        {
            if (childA != null || childB != null)
            {
                //Already split, go no further;
                return false;
            }

            bool splitHorizontal = RollPercentage(splitHorizontalChance);

            if (width > height && width / height >= 1.25)
                splitHorizontal = false;
            else if (height > width && height / width >= 1.25)
                splitHorizontal = true;

            int maxLeafSize = (splitHorizontal ? height : width) - minLeafSize;
            if (maxLeafSize <= minLeafSize)
            {
                return false;
            }

            int splitPosition = Random.Range(minLeafSize, maxLeafSize);

            if (splitHorizontal)
            {
                childA = new BSPLeaf(origin, width, splitPosition);
                GridPosition childBOrigin = new GridPosition(origin.x, origin.y + splitPosition);
                childB = new BSPLeaf(childBOrigin, width, height - splitPosition);
            }
            else
            {
                childA = new BSPLeaf(origin, splitPosition, height);
                GridPosition childBOrigin = new GridPosition(origin.x + splitPosition, origin.y);
                childA = new BSPLeaf(childBOrigin, width - splitPosition, height);

            }
            //Division successful
            return true;


        }

        public bool RollPercentage(int chanceToHit)
        {
            int randomResult = Random.Range(0, 100);
            if (randomResult < chanceToHit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

}
