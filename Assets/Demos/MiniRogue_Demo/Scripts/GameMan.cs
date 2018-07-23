using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;
using UnityEngine.Tilemaps;
using System;

public class GameMan : MonoBehaviour
{
    public bool playersTurn = true;
    public Entity[,] entityGrid;

    public BoardGenerator boardGenerator;

    public EntityDefinition[] entityDefinitions;

    private Dictionary<TileBase, EntityDefinition> TileEntityDefinitionDictionary = new Dictionary<TileBase, EntityDefinition>();

    private GridPosition playerPosition;
    // Use this for initialization
    void Start()
    {
        boardGenerator.BuildLevel();
        SetupTileEntityDefinitionDictionary();
        SetupEntityGrid();
    }

    void SetupEntityGrid()
    {
        entityGrid = new Entity[boardGenerator.profile.boardHorizontalSize, boardGenerator.profile.boardVerticalSize];

        for (int x = 0; x < boardGenerator.profile.boardHorizontalSize; x++)
        {
            for (int y = 0; y < boardGenerator.profile.boardVerticalSize; y++)
            {
                char foundChar = boardGenerator.boardGridAsCharacters[x, y];
                entityGrid[x, y] = SetupEntitySpace(x,y, foundChar);
            }
        }
    }


    Entity SetupEntitySpace(int x, int y, char charId)
    {
        Entity newEntity = new Entity();

        EntityDefinition definition = MatchTileToEntityDefinition(boardGenerator.profile.boardLibrary.GetTileFromChar(charId));

        if (definition.canMove)
        {
            newEntity.components.Add(new Mover());
        }

        if (definition.canBeDamaged)
        {
            newEntity.components.Add(new Damageable());
        }

        if (definition.playerControlled)
        {
            newEntity.components.Add(new PlayerControllable());
        }


        return newEntity;
    }

    EntityDefinition MatchTileToEntityDefinition(TileBase tile)
    {
        if (TileEntityDefinitionDictionary.ContainsKey(tile))
        {
            return TileEntityDefinitionDictionary[tile];
        }
        else
        {
            Debug.LogError("No entry for " + tile.name);
            return null;
        }
    }

    void SetupTileEntityDefinitionDictionary()
    {
        for (int i = 0; i < entityDefinitions.Length; i++)
        {
            for (int j = 0; j < entityDefinitions[i].tiles.Length; j++)
            {
                TileEntityDefinitionDictionary.Add(entityDefinitions[i].tiles[j], entityDefinitions[i]);
            }
            
        }
    }


    void EvaluateGrid()
    {

    }

    /*
    GridPosition FindPlayerEntityInGrid<T>(Type inputType)
    {
        for (int x = 0; x < boardGenerator.profile.boardHorizontalSize; x++)
        {
            for (int y = 0; y < boardGenerator.profile.boardVerticalSize; y++)
            {
                for (int z = 0; z < entityGrid[x,y].components.Count; z++)
                {
                    Type inputType = T.GetType();
                    Type typeFromGrid = entityGrid[x, y].components[z].GetType();
                    if (typeFromGrid == inputType)
                    {

                    }

                }
            }
        }
    }

    public void AcceptPlayerInput(int x, int y)
    {
        //GridPosition playerPosition = new
        //Debug.Log()
    }
    */
}
