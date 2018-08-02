using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnCollision : MonoBehaviour {

    public GameEffect gameEffect;
    public bool deactivateOnTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameEffect.TriggerEffect(collision.gameObject))
        {
            gameObject.SetActive(false);
        }
    }
}
