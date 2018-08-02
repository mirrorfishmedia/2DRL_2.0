using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using Pathfinding;

public class GameMan : MonoBehaviour
{
    public Transform player;
    public BoardGenerator boardGenerator;
    public AstarPath aStarPath;
    public SoundMan soundMan;

    public int goldTotal;
    public SoundEffect goldCollectSound;

    public UIController uiController;

    public static GameMan gm = null;


    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
    }

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

    public void CollectGold(int amount)
    {
        goldTotal += amount;
        uiController.UpdateCoinText(goldTotal);
        soundMan.PlaySoundEffect(goldCollectSound);
    }
}
