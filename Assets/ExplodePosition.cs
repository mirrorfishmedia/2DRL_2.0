using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Strata;

public class ExplodePosition : MonoBehaviour {

    public Vector2 offset;
    public Vector2 size = Vector2.one;
    Collider2D[] foundColliders = new Collider2D[32];
    private DamageSource damageSource;

    private void OnEnable()
    {
        damageSource = GetComponent<DamageSource>();
        int numColliders = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + offset, size, 0, foundColliders);
        for (int i = 0; i < numColliders; i++)
        {

            //DamageEnemies(i);
            DamageWalls(i);
        }


    }

    void DamageWalls(int index)
    {
        //Debug.Log("damaging walls");
        TilemapCollider2D tilemapCollider = foundColliders[index].GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)

        {
            //Debug.Log("pass");
            BoardGenerator boardGenerator = tilemapCollider.gameObject.GetComponent<BoardGenerator>();
            float xRaw = transform.position.x;
            float yRaw = transform.position.y;


            GridPosition roundedGrid = new GridPosition(Mathf.FloorToInt(xRaw), Mathf.FloorToInt(yRaw));

            roundedGrid.x = Mathf.Clamp(roundedGrid.x, 0, boardGenerator.profile.boardHorizontalSize);
            roundedGrid.y = Mathf.Clamp(roundedGrid.y, 0, boardGenerator.profile.boardVerticalSize);
            Debug.Log("roundedGrid" + roundedGrid.x + " " + roundedGrid.y);
            char emptyChar = boardGenerator.profile.boardLibrary.GetDefaultEmptyChar();
            boardGenerator.WriteToBoardGrid(roundedGrid.x, roundedGrid.y, emptyChar, true, true);
            boardGenerator.CreateMapEntryFromGrid(boardGenerator.boardGridAsCharacters[roundedGrid.x, roundedGrid.y], roundedGrid.GridPositionToVector2(roundedGrid));

        }
    }

    void DamageEnemies(int index)
    {

        Debug.Log("damaging enemies");
        EnemyDamageHandler handle = foundColliders[index].GetComponent<EnemyDamageHandler>();
        if (handle != null)
        {
            handle.HandleDamage(damageSource);
        }


    }
}
