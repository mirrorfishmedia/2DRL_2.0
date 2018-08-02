using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackSource : MonoBehaviour
{

    private Rigidbody2D myRb2d;

    private void Awake()
    {
        myRb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D hitObjRb2d = collision.gameObject.GetComponent<Rigidbody2D>();
        if (hitObjRb2d != null)
        {
            hitObjRb2d.AddForce(myRb2d.velocity, ForceMode2D.Impulse);
        }

    }
}
