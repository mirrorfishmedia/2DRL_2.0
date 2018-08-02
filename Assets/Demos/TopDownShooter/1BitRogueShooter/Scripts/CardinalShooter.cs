using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalShooter : MonoBehaviour

{
    public GameObject bulletPrefab;
    public float fireRate = .1f;
    public float bulletForce = 10f;
    public SoundEffect soundEffect;

    float nextFireTime;

    GameMan gameMan;

    private void Awake()
    {
        gameMan = FindObjectOfType<GameMan>();
    }

    // Update is called once per frame
    void Update ()
    {
       
        Vector2 shootDir;
        float x = Input.GetAxisRaw("HorizontalFire");
        float y = Input.GetAxisRaw("VerticalFire");

        shootDir = new Vector2(x, y);

        if (Time.time > nextFireTime)
        {
            
            if (x != 0 || y != 0)
            {
                Shoot(shootDir);
            }
            nextFireTime = Time.time + fireRate;
        }
        
	}

    void Shoot(Vector2 shootDir)
    {
        gameMan.soundMan.PlaySoundEffect(soundEffect);
        Vector2 spawnPos = (Vector2) transform.position + (shootDir.normalized * 1.1f);
        GameObject cloneBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb2d = cloneBullet.GetComponent<Rigidbody2D>();
        rb2d.AddForce(shootDir * bulletForce, ForceMode2D.Impulse);
    }

}
