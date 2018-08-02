using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour {

    public GameObject deathPersistentObject;
    public EnemyStats enemyStats;
    private FlashSprite flashSprite;
    public GameEffect[] deathEffects;
    public GameEffect damageEffect;
    private int currentHp;

    private void Start()
    {
        flashSprite = GetComponent<FlashSprite>();
        currentHp = enemyStats.hitPoints;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageSource dmgSource = collision.gameObject.GetComponent<DamageSource>();
        if (dmgSource != null)
        {

            HandleDamage(dmgSource);
        }
    }

    void HandleDamage(DamageSource damageSource)
    {
        currentHp -= damageSource.damageAmount;
        flashSprite.TriggerFlash();
        CheckIfDead();
        Debug.Log("current hp " + currentHp);
    }

    void CheckIfDead()
    {
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    void Die()
    {
        for (int i = 0; i < deathEffects.Length; i++)
        {
            deathEffects[i].TriggerEffect(this.gameObject);
        }
       
        //deathPersistentObject.SetActive(true);
        //deathPersistentObject.transform.SetParent(null);
        this.gameObject.SetActive(false);
    }
}
