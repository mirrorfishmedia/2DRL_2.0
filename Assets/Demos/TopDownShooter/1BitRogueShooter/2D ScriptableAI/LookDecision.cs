using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision {

	public override bool Decide(StateController controller)
	{
		bool targetVisible = Look(controller);
		return targetVisible;
	}

	private bool Look(StateController controller)
	{
		RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, controller.facingDir, controller.enemyStats.lookRange, controller.sightFilterMask);


		Debug.DrawRay (controller.eyes.position, controller.facingDir * controller.enemyStats.lookRange, Color.magenta);

        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Player"))
            {
                controller.chaseTarget = hit.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
			
	}
}
