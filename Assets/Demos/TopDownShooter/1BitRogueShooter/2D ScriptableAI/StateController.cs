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
    public Vector2 facingDir;
    public GameObject[] artHolders;
    public LayerMask sightFilterMask;
    public DirectionalShooter shooter;
    public MeleeAttack meleeAttack;

	private bool aiActive;

	void Awake () 
	{
		aIDestinationSetter = GetComponent<AIDestinationSetter> ();
        aiLerp = GetComponent<AILerp>();
        aiActive = true;
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
        SetFacingDir();

    }

    void SetFacingDir()
    {
        if (dirToChaseTarget.x > 0)
        {
            facingDir = new Vector2(1, 0);
            for (int i = 0; i < artHolders.Length; i++)
            {
                artHolders[i].transform.localScale = new Vector3(1, 1, 1);

            }
        }
        else
        {
            facingDir = new Vector2(-1, 0);
            for (int i = 0; i < artHolders.Length; i++)
            {
                artHolders[i].transform.localScale = new Vector3(-1, 1, 1);

            }
        }
    }

	void OnDrawGizmos()
	{
		if (currentState != null && eyes != null) 
		{
			Gizmos.color = currentState.sceneGizmoColor;
            Debug.DrawRay(eyes.position, dirToChaseTarget);
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