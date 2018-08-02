using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyDamageHandler : MonoBehaviour {

    public GameObject deathPersistentObject;
    public EnemyStats enemyStats;
    private FlashSprite flashSprite;
    public GameEffect[] deathEffects;
    public GameEffect damageEffect;
    private int currentHp;
    private AILerp aiLerp;
    bool dead;

    public float damagedInterval = .25f;
    bool inDamagedState;
    private WaitForSeconds damagedWait;

    private void Awake()
    {
        damagedWait = new WaitForSeconds(damagedInterval);
        flashSprite = GetComponent<FlashSprite>();
        currentHp = enemyStats.hitPoints;
        aiLerp = GetComponent<AILerp>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageSource dmgSource = collision.gameObject.GetComponent<DamageSource>();
        if (dmgSource != null)
        {

            HandleDamage(dmgSource);
        }
    }

    IEnumerator DamagedState()
    {
        if (aiLerp != null)
        {
            aiLerp.enabled = false;
            //aiLerp.updatePosition = false;
            //aiLerp.canMove = false;
            yield return damagedWait;
            aiLerp.enabled = true;
            //aiLerp.updatePosition = true;
            //aiLerp.canMove = true;

        }
    }

    void HandleDamage(DamageSource damageSource)
    {
        currentHp -= damageSource.damageAmount;
        flashSprite.TriggerFlash();

        StartCoroutine(DamagedState());
        CheckIfDead();

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
            deathEffects[i].TriggerEffect(this.gameObject, this.gameObject);
        }
       
        deathPersistentObject.SetActive(true);
        deathPersistentObject.transform.SetParent(null);


        this.gameObject.SetActive(false);
    }
}
