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
		RaycastHit2D hit;

		Debug.DrawRay (controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);

		if (Physics2D.Raycast (controller.eyes.position, controller.dirToChaseTarget, controller.enemyStats.lookRange, out hit)
		    && hit.collider.CompareTag ("Player")) {
			controller.chaseTarget = hit.transform;
			return true;
		} else 
		{
			return false;
		}
			
	}


}
