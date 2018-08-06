using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnCollision : MonoBehaviour {

    public GameEffect gameEffect;
    public Item item;
    public bool deactivateOnTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameEffect.TriggerEffect(collision.gameObject, this.gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameEffect.TriggerEffect(collision.gameObject, this.gameObject);
       
    }
}
