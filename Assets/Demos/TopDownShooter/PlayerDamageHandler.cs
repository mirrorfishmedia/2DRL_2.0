using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour {


    private FlashSprite flashSprite;
    public GameEffect damageEffect;
    public int maxHp = 10;
    private int currentHp;
    bool dead;

    public float damagedInterval = .125f;
    bool inDamagedState;
    private WaitForSeconds damagedWait;
    private PlayerMoverSimple playerMoverSimple;

    private void Awake()
    {
        damagedWait = new WaitForSeconds(damagedInterval);
        flashSprite = GetComponent<FlashSprite>();
        currentHp = maxHp;
        playerMoverSimple = GetComponent<PlayerMoverSimple>();
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
        if (playerMoverSimple != null)
        {
            playerMoverSimple.enabled = false;
            yield return damagedWait;
            playerMoverSimple.enabled = true;

        }
    }

    public void HandleDamage(DamageSource damageSource)
    {
        Debug.Log("handleDamage dmgSource " + damageSource);
        currentHp -= damageSource.damageAmount;
        flashSprite.TriggerFlashExpire();

        StartCoroutine(DamagedState());
        CheckIfDead();
        GameMan.gm.DisplayHealth(currentHp);


    }

    private void CheckIfDead()
    {
        if (currentHp <= 0)
        {
            playerMoverSimple.enabled = false;
            GameMan.gm.EndGame();
        }
        

    }


}
