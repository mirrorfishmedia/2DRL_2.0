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
		//RaycastHit hit;

		Debug.DrawRay (controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, controller.dirToChaseTarget, controller.enemyStats.attackRange);


        if (hit.collider.CompareTag("Player"))
		{
			if (controller.CheckIfCountDownElapsed (controller.enemyStats.attackRate)) 
			{
                //shoot here
				//controller.tankShooting.Fire (controller.enemyStats.attackForce, controller.enemyStats.attackRate);
			}
		}
	}
}
