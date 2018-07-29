using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        //Debug.Log("nextWaypoint " + controller.nextWayPoint + "controller.wayPointList.Count " + controller.wayPointList.Count);
        if (controller.wayPointList.Count > 0)
        {
            controller.aIDestinationSetter.target = controller.wayPointList[controller.nextWayPoint];

            if (controller.aiLerp.reachedEndOfPath)
            {
                controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
            }
        }
        
    }
}
