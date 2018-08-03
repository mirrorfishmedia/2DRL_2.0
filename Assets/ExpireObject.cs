using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpireObject : MonoBehaviour {
    
    public float expireTime = 8f;

    private FlashSprite flashSprite;
    private WaitForSeconds expireWait;

    private void Awake()
    {
        flashSprite = GetComponent<FlashSprite>();
        expireWait = new WaitForSeconds(expireTime);
        
    }

    IEnumerator Expire()
    {
        yield return expireWait;
        if (flashSprite != null)
        {
            yield return flashSprite.FlashBeforeExpire();

        }
        Debug.Log("deactivating time.time " + Time.time);
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void OnEnable ()
    {
        StartCoroutine(Expire());	
	}
	
}
