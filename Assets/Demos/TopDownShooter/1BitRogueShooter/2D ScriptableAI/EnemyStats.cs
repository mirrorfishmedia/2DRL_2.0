using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int hitPoints = 3;
	public float moveSpeed = 1;
	public float lookRange = 40f;
	public Vector2 lookCapsuleDimensions = new Vector2 (1f,5f);

	public float attackRange = 1f;
	public float fireRate = 1f;
	public float attackForce = 15f;
	public int attackDamage = 50;

	public float searchDuration = 4f;
	public float searchingTurnSpeed = 120f;
}