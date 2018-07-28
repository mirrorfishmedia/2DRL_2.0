using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieOnHit : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageSource source = collision.gameObject.GetComponent<DamageSource>();
        if (source != null)
        {
            gameObject.SetActive(false);
        }
    }
}
