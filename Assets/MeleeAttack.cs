using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    public Transform attackPos;

    public float attackRate = .25f;
    public Vector2 attackBoxSize = new Vector2 (.25f, .5f);

    private DamageSource damageSource;
    private SpriteSwapper spriteSwapper;
    private float nextAttackTime;

    private void Awake()
    {
        spriteSwapper = GetComponent<SpriteSwapper>();
        damageSource = GetComponent<DamageSource>();
    }

    public void TriggerAttack()
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            spriteSwapper.TriggerSwap();
            Collider2D hitCol = Physics2D.OverlapBox(attackPos.position, attackBoxSize, 0);
            if (hitCol != null)
            {
                EnemyDamageHandler damageHandler = hitCol.GetComponent<EnemyDamageHandler>();
                if (damageHandler != null)
                {
                    damageHandler.HandleDamage(damageSource);
                }
            }
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (attackPos)
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackBoxSize.x, attackBoxSize.y,attackBoxSize.x));
    }
}
