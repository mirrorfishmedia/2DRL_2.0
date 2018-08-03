using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using Pathfinding;
using System;

public class GameMan : MonoBehaviour
{
    public Transform player;
    public BoardGenerator boardGenerator;
    public AstarPath aStarPath;
    public SoundMan soundMan;

    public int goldTotal;
    public SoundEffect goldCollectSound;

    public BoardGenerationProfile[] boardGenerationProfiles;

    public UIController uiController;

    public static GameMan gm = null;

    public List<GameObject> destroyEverythingList = new List<GameObject>();

    public int levelNumber = 0;

    public GameObject gameOverText;
    

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
        DontDestroyOnLoad(this.gameObject);
        soundMan = GetComponent<SoundMan>();
        StartCoroutine(Setup());	
	}

    public void ScheduleForDestruction(GameObject toDestroy)
    {
        destroyEverythingList.Add(toDestroy);
    }

    public void DestroyEverything()
    {
        for (int i = destroyEverythingList.Count - 1; i > -1 ; i--)
        {
            Destroy(destroyEverythingList[i]);
        }

        System.GC.Collect();
    }

    public void ChangeLevels()
    {
        //levelNumber++;

        boardGenerator.profile = boardGenerationProfiles[levelNumber];
        DestroyEverything();
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        boardGenerator.ClearLevel();
        yield return StartCoroutine(boardGenerator.BuildLevel());
        aStarPath.Scan();
    }

    public void CollectGold(int amount)
    {
        goldTotal += amount;
        uiController.UpdateCoinText(goldTotal);
        soundMan.PlaySoundEffect(goldCollectSound);
    }

    public void DisplayHealth(int totalHealth)
    {
        uiController.UpdateHealthText(totalHealth);
    }

    public void EndGame()
    {
        gameOverText.SetActive(true);
    }
}
