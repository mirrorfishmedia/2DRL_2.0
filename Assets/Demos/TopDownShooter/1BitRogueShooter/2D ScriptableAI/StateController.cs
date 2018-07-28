using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class StateController : MonoBehaviour {

	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;
	public State remainState;



	[HideInInspector] public AIDestinationSetter aIDestinationSetter;
	[HideInInspector] public List<Transform> wayPointList;
	[HideInInspector] public int nextWayPoint;
	[HideInInspector] public Transform chaseTarget;
	[HideInInspector] public float stateTimeElapsed;

    public AILerp aiLerp;
    public Vector2 dirToChaseTarget;

	private bool aiActive;

	void Awake () 
	{
		aIDestinationSetter = GetComponent<AIDestinationSetter> ();
        aiLerp = GetComponent<AILerp>();

    }

	public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
	{
        /*
		wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
		if (aiActive) 
		{
			navMeshAgent.enabled = true;
		} else 
		{
			navMeshAgent.enabled = false;
		}
        */
	}

	void Update()
	{
		if (!aiActive)
			return;
		currentState.UpdateState (this);

        if (aIDestinationSetter.target != null)
        {
            dirToChaseTarget = aIDestinationSetter.target.position - transform.position;
        }
        
	}

	void OnDrawGizmos()
	{
		if (currentState != null && eyes != null) 
		{
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere (eyes.position, enemyStats.lookSphereCastRadius);
		}
	}

	public void TransitionToState(State nextState)
	{
		if (nextState != remainState) 
		{
			currentState = nextState;
			OnExitState ();
		}
	}

	public bool CheckIfCountDownElapsed(float duration)
	{
		stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
	}

	private void OnExitState()
	{
		stateTimeElapsed = 0;
	}
}