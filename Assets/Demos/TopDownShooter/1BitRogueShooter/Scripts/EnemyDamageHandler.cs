using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour {

    public DamageEffect dmgEffect;
    public GameObject deathPersistentObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageSource dmgSource = collision.gameObject.GetComponent<DamageSource>();
        if (dmgSource != null)
        {
            dmgEffect.HandleDamage(this.gameObject);
            deathPersistentObject.SetActive(true);
            deathPersistentObject.transform.SetParent(null);

        }
    }
}
