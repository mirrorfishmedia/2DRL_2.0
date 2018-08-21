﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text coinAmountText;
    public Text healthAmountText;

	// Use this for initialization
	void Start ()
    {
        UpdateCoinText(GameMan.gm.goldTotal);
    }

    public void UpdateCoinText(int amount)
    {
        coinAmountText.text = amount.ToString();
    }

    public void UpdateHealthText(int healthTotal)
    {
        healthAmountText.text = healthTotal.ToString();
    }
}