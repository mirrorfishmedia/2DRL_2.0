using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (menuName = "MiniRogueDemo/EntityDefinition")]
public class EntityDefinition : ScriptableObject
{
    public TileBase[] tiles;
    public bool canMove;
    public bool canBeDamaged;
    public bool playerControlled;
    public bool isExit;
}
