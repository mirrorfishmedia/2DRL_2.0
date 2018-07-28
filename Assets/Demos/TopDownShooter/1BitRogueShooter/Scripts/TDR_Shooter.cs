using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDR_Shooter : MonoBehaviour
{
    public Vector2 shootDir;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 5f;

    public void Shoot()
    {
        GameObject clone = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation) as GameObject;
        Rigidbody2D rb2d = clone.GetComponent<Rigidbody2D>();
        rb2d.AddForce(shootDir * bulletSpeed);
    }
}
