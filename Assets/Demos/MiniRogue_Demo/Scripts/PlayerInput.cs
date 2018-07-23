using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;


public class PlayerInput : MonoBehaviour
{
    Mover mover;
    GameMan gameMan;

    void Awake()
    {
        gameMan = GetComponent<GameMan>();
    }

    private void Update()
    {
        /*
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (verticalMove != 0f)
        {
            if (verticalMove > 0f)
            {
                gameMan.AcceptPlayerInput(0, 1);
            }
            else if (verticalMove < 0f)
            {
                gameMan.AcceptPlayerInput(0, -1);
            }

        }
        else if (horizontalMove > 0)
        {
            gameMan.AcceptPlayerInput(1, 0);
        }
        else if (horizontalMove < 0)
        {
            gameMan.AcceptPlayerInput(-1, 0);
        }
        */

    }

    void MovementTest()
    {

    }

}
