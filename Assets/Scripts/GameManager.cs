using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public bool playersTurn;
	public int currentCoins = 0;
	public int currentFood = 0;
	public Text coinDisplay;
	public Text foodDisplay;

	public static GameManager instance;

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

	// Update is called once per frame
	void Update () {
		
	}
}
