using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreader : MonoBehaviour {

    public float spreadRate = .1f;
    public int fuelMax = 6;
    public int currentFuel = 15;
    public GameObject spreadingPrefab;
    public float chanceToDieOut = .25f;
    public SpriteRenderer renderer;

    float nextTimeInterval;
    public Vector2[] surroundingSquares;
    public LayerMask layersToCheck;

	// Use this for initialization
	void OnEnable ()
    {
        currentFuel = Random.Range(fuelMax/2, fuelMax);
        nextTimeInterval = Time.time + spreadRate;	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Time.time > nextTimeInterval)
        {
            nextTimeInterval = Time.time + spreadRate;
            currentFuel--;
            CheckExpire();
            Spread();
        }
	}

    void CheckExpire()
    {
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            gameObject.SetActive(false);
            renderer.enabled = false;
        }
    }

    void Spread()
    {
        for (int i = 0; i < surroundingSquares.Length; i++)
        {
            if (i != 4)
            {
                if (!CheckSquareViaRaycast(surroundingSquares[i]))
                {
                    float rollToDieOut = Random.Range(0f, 1f);
                    if (rollToDieOut > chanceToDieOut)
                    {
                        GameObject clone = Instantiate(spreadingPrefab, (Vector2)transform.position + surroundingSquares[i], Quaternion.identity);
                    }
                }
            }
        }

    }

    bool CheckSquareViaRaycast(Vector2 direction)
    {
        RaycastHit2D[] results = new RaycastHit2D[4];
        int objectsDetected = Physics2D.RaycastNonAlloc(transform.position, direction, results, 1f, layersToCheck);
        bool detectedObject;
        if (objectsDetected > 1)
        {
            detectedObject = true;
        }
        else
        {
            detectedObject = false;
        }
        return detectedObject;
    }
}
