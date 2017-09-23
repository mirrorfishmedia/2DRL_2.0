using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool playersTurn;
    public bool enemiesMoving;
	public int currentCoins = 0;
	public int currentFood = 0;
	public Text coinDisplay;
	public Text foodDisplay;
    public EnemyController enemyController;
    public Transform player;
    public BoardGenerator boardGenerator;
    public float turnDelay = 0.1f;

	public static GameManager instance;

    private WaitForSeconds waitBetweenTurns;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null) {
			instance = this;
		} else 
		{
			Destroy (this.gameObject);
		}
	}

    private void Start()
    {
        waitBetweenTurns = new WaitForSeconds(turnDelay);
        playersTurn = true;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving)
        {
            return;
        }

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        enemiesMoving = true;

        enemyController.UpdateEnemies(boardGenerator);
        yield return waitBetweenTurns;
        playersTurn = true;
        enemiesMoving = false;
    }

    public void ModifyCoins(int value)
	{
		currentCoins += value;
		coinDisplay.text = currentCoins.ToString ();
	}

	public void ModifyFood(int value)
	{
		currentFood += value;
		foodDisplay.text = currentFood.ToString ();
	}
    
}