using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;

[CreateAssetMenu(menuName = "1BitRogue/Effects/TeleportEffect")]
public class TeleportEffect : GameEffect
{
    public SoundEffect teleportSound;

    public override bool TriggerEffect(GameObject triggeringObject)
    {
        BoardGenerator boardGenerator = FindObjectOfType<BoardGenerator>();
        GameMan gameMan = FindObjectOfType<GameMan>();
        gameMan.soundMan.PlaySoundEffect(teleportSound);

        GridPosition randomEmptyPosition = boardGenerator.GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(boardGenerator);
        Vector2 emptySpaceToTeleportTo = randomEmptyPosition.GridPositionToVector2(randomEmptyPosition);
        triggeringObject.transform.position = emptySpaceToTeleportTo;
        return true;
    }
}
