using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoverSimple : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float moveSpeed = .5f;

    Vector2 moveDir;

    public GameObject artHolder;

	// Use this for initialization
	void Awake ()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float verticalMove = Input.GetAxis("Vertical");
        float horizontalMove = Input.GetAxis("Horizontal");
        SetFacingDir(horizontalMove);
        moveDir = new Vector2(horizontalMove * moveSpeed, verticalMove * moveSpeed);

       
	}

    void SetFacingDir(float horizontalDir)
    {
        if (horizontalDir < 0)
        {
            //facingDir = new Vector2(1, 0);
            artHolder.transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontalDir > 0)
        {
            //facingDir = new Vector2(-1, 0);
            artHolder.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition((Vector2)transform.position + moveDir);
    }
}
