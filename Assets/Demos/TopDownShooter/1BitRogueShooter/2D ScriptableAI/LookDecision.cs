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
        Collider2D[] overlapColliders = new Collider2D[32];
        int foundColliders = Physics2D.OverlapCircleNonAlloc(controller.transform.position, controller.enemyStats.lookRange, overlapColliders);
        Vector2 playerPosition = Vector2.zero;

        for (int i = 0; i < foundColliders; i++)
        {
            if (overlapColliders[i].CompareTag("Player"))
            {
                playerPosition = overlapColliders[i].transform.position;
            }
        }

        Vector2 dirToDetectedPlayer = playerPosition - (Vector2)controller.transform.position;

        if (playerPosition != Vector2.zero)
        {
            RaycastHit2D hit = Physics2D.Raycast(controller.eyes.position, dirToDetectedPlayer, controller.enemyStats.lookRange, controller.sightFilterMask);

            Debug.DrawRay(controller.eyes.position, dirToDetectedPlayer * controller.enemyStats.lookRange, Color.magenta);

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
        }

		
        return false;
			
	}
}
