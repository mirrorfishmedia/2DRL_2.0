using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{

    public Material flashSpriteMat;
    public Material originalMat;

    public SpriteRenderer spriteRenderer;

    private WaitForSeconds flashWait = new WaitForSeconds(.25f);


    public void TriggerFlash()
    {
        StartCoroutine(Flash());
    }


    public IEnumerator Flash()
    {
        spriteRenderer.material = flashSpriteMat;
        yield return flashWait;
        spriteRenderer.material = originalMat;
    }
	
}
