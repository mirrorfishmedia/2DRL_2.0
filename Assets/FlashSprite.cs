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


    public void FlashOnce()
    {

    }

    public void TriggerFlash()
    {
        StartCoroutine(Flash());
    }

    public void TriggerFlashExpire()
    {
        StartCoroutine(FlashBeforeExpire());
    }

    public IEnumerator FlashBeforeExpire()
    {
        int count = 0;
        
        while (count <= numFlashes)
        {

            yield return flashWait;
            spriteRenderer.enabled = !spriteRenderer.enabled;
            count++;
        }
        //spriteRenderer.enabled = true;
    }

    public IEnumerator Flash()
    {
        spriteRenderer.material = flashSpriteMat;
        yield return flashWait;
        spriteRenderer.material = originalMat;
    }
	
}
