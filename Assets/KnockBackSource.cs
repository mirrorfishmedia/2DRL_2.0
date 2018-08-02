using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackSource : MonoBehaviour
{

    private Rigidbody2D myRb2d;
    public float forceMultiplier = .5f;

    private void Awake()
    {
        myRb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D hitObjRb2d = collision.gameObject.GetComponent<Rigidbody2D>();
        if (hitObjRb2d != null)
        {
            hitObjRb2d.AddForce(myRb2d.velocity * forceMultiplier, ForceMode2D.Impulse);
        }

    }
}
