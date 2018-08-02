using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour {

    public AudioClip[] musicLoops;
    private AudioSource source;

	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
        source.clip = musicLoops[Random.Range(0, musicLoops.Length)];
        source.Play();
	    	
	}
}
