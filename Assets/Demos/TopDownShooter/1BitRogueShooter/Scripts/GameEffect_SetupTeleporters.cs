using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;

[CreateAssetMenu(menuName = "1BitRogue/Effects/SetupTeleporters")]

public class GameEffect_SetupTeleporters : GameEffect
{
    public GameObject teleporterEntrance;
    public GameObject teleporterExit;
    public SoundEffect setupTeleporterSound;

    public override bool TriggerEffect(GameObject triggeringObject)
    {
        GameObject entrance = Instantiate(teleporterEntrance, triggeringObject.transform.position, Quaternion.identity);
        entrance.name = "entrance";
        
        GameObject exit = Instantiate(teleporterExit, triggeringObject.transform.position, Quaternion.identity);
        exit.name = "exit";
        LinkedTeleporter link = entrance.GetComponent<LinkedTeleporter>();
        link.linkedTeleporter = exit.GetComponent<LinkedTeleporter>();
        link.linkedPosition = exit.transform;

        SetupExit(entrance,exit, link);
        return true;
    }

    public void SetupExit(GameObject entrance, GameObject exit, LinkedTeleporter linkBack)
    {
        BoardGenerator boardGenerator = FindObjectOfType<BoardGenerator>();
        GameMan gameMan = FindObjectOfType<GameMan>();
        gameMan.soundMan.PlaySoundEffect(setupTeleporterSound);
        LinkedTeleporter link = exit.GetComponent<LinkedTeleporter>();
        link.linkedTeleporter = entrance.GetComponent<LinkedTeleporter>();
        link.linkedPosition = entrance.transform;

        //linkBack.linkedTeleporter = entrance.transform;

        GridPosition randomEmptyPosition = boardGenerator.GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(boardGenerator);
        Vector2 emptySpaceToTeleportTo = randomEmptyPosition.GridPositionToVector2(randomEmptyPosition);
        exit.transform.position = emptySpaceToTeleportTo;
    }
}
