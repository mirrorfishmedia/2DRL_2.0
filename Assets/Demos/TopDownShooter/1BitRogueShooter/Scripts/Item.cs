using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Item")]

public class Item : ScriptableObject
{
    public Sprite itemIconSprite;
    public GameEffect gameEffect;
}
