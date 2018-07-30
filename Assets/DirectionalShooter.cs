using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalShooter : MonoBehaviour {

    public GameObject prefabToSpawn;
    public float shootForce = 2f;
    public float fireRate = 1f;


    private float nextFireTime;
    private float colliderDelay = 1.2f;
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
            nextFireTime = Time.time + fireRate;
            GameObject clone = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Rigidbody2D rb2d = clone.GetComponent<Rigidbody2D>();
            rb2d.AddForce(stateController.dirToChaseTarget.normalized * shootForce, ForceMode2D.Impulse);
            gameObject.layer = 12;
            Invoke("SetColliderBackToEnemyLayer", colliderDelay);
        }
        
    }

    void SetColliderBackToEnemyLayer()
    {
        gameObject.layer = 11;
        Debug.Log("layer b" + gameObject.layer);
    }

	
}
