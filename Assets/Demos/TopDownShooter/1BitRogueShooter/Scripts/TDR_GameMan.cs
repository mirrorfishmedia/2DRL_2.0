﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using Pathfinding;

public class TDR_GameMan : MonoBehaviour
{
    public Transform player;
    public BoardGenerator boardGenerator;
    public AstarPath aStarPath;
    public SoundMan soundMan;

	// Use this for initialization
	void Start ()
    {
        soundMan = GetComponent<SoundMan>();
        StartCoroutine(Setup());	
	}
	

    IEnumerator Setup()
    {
        yield return StartCoroutine(boardGenerator.BuildLevel());
        aStarPath.Scan();
    }
}
