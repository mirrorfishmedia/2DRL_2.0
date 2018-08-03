using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour {

    //public Sprite[] sprites;
    public GameObject idle;
    public GameObject attack;
    public float swapLength = .25f;

    private WaitForSeconds swapWait;

    int spriteIndex;
	// Use this for initialization
	void Start ()
    {
        swapWait = new WaitForSeconds(swapLength);	
	}

    public void TriggerSwap()
    {
        StartCoroutine(Swap());
    }

    IEnumerator Swap()
    {
        idle.SetActive(false);

        attack.SetActive(true);
        yield return swapWait;
        idle.SetActive(true);

        attack.SetActive(false);
    }
    
}
