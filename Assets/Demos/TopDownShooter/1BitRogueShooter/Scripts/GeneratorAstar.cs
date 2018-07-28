using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using Pathfinding;

[CreateAssetMenu(menuName = "Strata/Generators/AstarPath")]
public class GeneratorAstar : Generator
{
    
    public override bool Generate(BoardGenerator boardGenerator)
    {
        AstarPath aStarPath;
        aStarPath = GameObject.FindObjectOfType<AstarPath>();
        if (aStarPath != null)
        {
            aStarPath.Scan();
        }


        return true;
    }
}
