using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{

    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageSource source = collision.gameObject.GetComponent<DamageSource>();
        if (source == null)
        {
            this.gameObject.SetActive(false);
        }
        
    }
}
