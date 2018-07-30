using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action 
{
	public override void Act (StateController controller)
	{
		Attack (controller);
	}

	private void Attack(StateController controller)
	{
        RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, controller.dirToChaseTarget, controller.enemyStats.lookRange, controller.sightFilterMask);

        Debug.DrawRay(controller.eyes.position, controller.dirToChaseTarget * controller.enemyStats.lookRange, Color.yellow);

        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Player"))
            {
                controller.shooter.Shoot(controller.dirToChaseTarget);
            }

        }
	}
}
