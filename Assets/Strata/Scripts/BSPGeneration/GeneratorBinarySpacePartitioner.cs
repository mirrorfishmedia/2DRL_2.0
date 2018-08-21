using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Generators/Generator Binary Space Partitioner")]
    public class GeneratorBinarySpacePartitioner : Generator
    {
        public int maxLeafSize = 20;

        private BSPLeaf leaf;

        public override bool Generate(BoardGenerator boardGenerator)
        {
            SplitLeaves(boardGenerator);
            return true;
        }

        public void SplitLeaves(BoardGenerator boardGenerator)
        {
            //Create the first root leaf that fills the whole board, we'll split this
            BSPLeaf rootLeaf = new BSPLeaf(new GridPosition(0, 0), boardGenerator.profile.boardHorizontalSize, boardGenerator.profile.boardVerticalSize);

            List<BSPLeaf> leafList = new List<BSPLeaf>();

            leafList.Add(rootLeaf);

            bool splitLeaf = true;

            Debug.Log("leafList count " + leafList.Count);

            while (splitLeaf)
            {
                splitLeaf = false;

                for (int i = 0; i < leafList.Count; i++)
                {
                    Debug.Log("leafList i " + i + "leaf origin " + leafList[i].origin.x + " " + leafList[i].origin.y);
                    if (leafList[i].childA == null && leafList[i].childB == null)
                    {
                        if (leafList[i].width > maxLeafSize || leafList[i].height > maxLeafSize || RollPercentage(75))
                        {
                            if (leafList[i].Split())
                            {
                                leafList.Add(leafList[i].childA);
                                leafList.Add(leafList[i].childB);
                                splitLeaf = true;
                            }
                        }
                    }
                }
            }

            DrawLeaves(boardGenerator, leafList);
        }

        public void DrawLeaves(BoardGenerator boardGenerator, List<BSPLeaf> leafList)
        {
            for (int i = 0; i < leafList.Count; i++)
            {
                Debug.Log("leafList " + leafList[i].origin + " height " + leafList[i].height + " width " + leafList[i].height);
            }
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

