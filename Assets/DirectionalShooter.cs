using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalShooter : MonoBehaviour {

    public GameObject prefabToSpawn;
    public float shootForce = 2f;
    public EnemyStats enemyStats;

    private float nextFireTime;
    private float colliderDelay = .5f;
    private StateController stateController;
    private Collider enemyCollider;
   

	// Use this for initialization
	void Awake ()
    {
        stateController = GetComponent<StateController>();
    }

    public void Shoot(Vector2 shootDir)
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + enemyStats.fireRate;
            GameObject clone = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Rigidbody2D rb2d = clone.GetComponent<Rigidbody2D>();
            rb2d.AddForce(stateController.dirToChaseTarget.normalized * enemyStats.attackForce, ForceMode2D.Impulse);
            gameObject.layer = 12;
            Invoke("SetColliderBackToEnemyLayer", colliderDelay);
        }
        
    }

    void SetColliderBackToEnemyLayer()
    {
        gameObject.layer = 11;
    }

	
}
