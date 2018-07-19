using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<PlayerPlatformerController>().transform;
        player.transform.position = this.transform.position;
        
	}

}
