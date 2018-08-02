using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{

    public Material flashSpriteMat;
    public Material originalMat;
    public Material flashClearMat;
    public int numFlashes;

    public SpriteRenderer spriteRenderer;

    private WaitForSeconds flashWait = new WaitForSeconds(.125f);

    public bool countdownFromEnable = false;




    public void TriggerFlash()
    {
        StartCoroutine(Flash());
    }

    public void TriggerFlashExpire()
    {
        Debug.Log("tfe");
        StartCoroutine(FlashBeforeExpire());
    }

    public IEnumerator FlashBeforeExpire()
    {
        Debug.Log("fbe");

        int count = 0;
        
        while (count <= numFlashes)
        {
            Debug.Log("count " + count);
            Debug.Log("clear time.time " + Time.time);

            Debug.Log("sprite renderer false " + spriteRenderer.enabled);
            yield return flashWait;
            Debug.Log("original time.time " + Time.time);
            spriteRenderer.enabled = !spriteRenderer.enabled;

            Debug.Log("sprite renderer true" + spriteRenderer.enabled);
            count++;
        }
       
    }

    public IEnumerator Flash()
    {
        Debug.Log("flash");
        spriteRenderer.material = flashSpriteMat;
        yield return flashWait;
        spriteRenderer.material = originalMat;
    }
	
}
