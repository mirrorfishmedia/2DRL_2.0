using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpireObject : MonoBehaviour {
    
    public float expireTime = 8f;
    public GameEffect disableEffect;

    private FlashSprite flashSprite;
    private WaitForSeconds expireWait;

    private void Awake()
    {
        flashSprite = GetComponent<FlashSprite>();
        expireWait = new WaitForSeconds(expireTime);
        
    }

    private void DisableEffect()
    {
        if (disableEffect != null)
        {
            disableEffect.TriggerEffect(this.gameObject, this.gameObject);
        }
    }

    IEnumerator Expire()
    {
        yield return expireWait;
        if (flashSprite != null)
        {
            yield return flashSprite.FlashBeforeExpire();

        }
        DisableEffect();
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void OnEnable ()
    {
        StartCoroutine(Expire());	
	}
	
}
