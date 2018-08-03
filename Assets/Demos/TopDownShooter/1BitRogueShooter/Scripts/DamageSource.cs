using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{

    public int damageAmount = 1;
    public bool disableOnCollision = true;
    public bool damageOnCollision = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageOnCollision)
        {
            DamageSource source = collision.gameObject.GetComponent<DamageSource>();
            if (disableOnCollision)
            {
                gameObject.SetActive(false);


            }
        }

    }
}
