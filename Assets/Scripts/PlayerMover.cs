using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

	public BoardGenerator boardGenerator;

	public float moveRate = .1f;

	private UnitData unitData;
	private float nextMoveTime;


	// Use this for initialization
	void Start () 
	{
		unitData = GetComponent<UnitData> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.


		//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

		//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
		vertical = (int) (Input.GetAxisRaw ("Vertical"));

		//Check if moving horizontally, if so set vertical to zero.
		if(horizontal != 0)
		{
			vertical = 0;
		}

		//Check if we have a non-zero value for horizontal or vertical
		if(horizontal != 0 || vertical != 0)
		{


            if (GameManager.instance.playersTurn)
            {
                TryMove(horizontal, vertical);
                GameManager.instance.playersTurn = false;
            }
				
			

		}
	}

	public void TryMove(int horizontal, int vertical)
	{
		Vector2 targetSpace = new Vector2 (horizontal, vertical) + (Vector2) transform.position;
        //MapCell currentCell = boardGenerator.tileData[(int)transform.position.x, (int)transform.position.y];
		if (SpaceOpen (targetSpace))
        {
			//set previous space back to floor
			boardGenerator.TrackMovingUnit (transform.position, 0);
			boardGenerator.TrackMovingUnit (transform.position, MapCell.CellType.BlackFloor);
			this.transform.position = targetSpace;
			boardGenerator.TrackMovingUnit (transform.position, unitData.tileType);
		}

	}

	bool SpaceOpen(Vector2 targetSpace)
	{
		int x = (int)targetSpace.x;
		int y = (int)targetSpace.y;
		return boardGenerator.CheckMapForObstruction (x, y);

	}

}

