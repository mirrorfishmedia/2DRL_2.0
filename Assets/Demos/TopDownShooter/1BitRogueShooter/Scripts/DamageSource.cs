using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{

    public int damageAmount = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }
}
