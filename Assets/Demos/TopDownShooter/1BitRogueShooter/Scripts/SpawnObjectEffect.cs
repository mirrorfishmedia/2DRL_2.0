using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/SpawnObject")]

public class SpawnObjectEffect : GameEffect {

    public GameObject[] objectsToSpawn;

    public override bool TriggerEffect(GameObject triggeringObject)
    {
        GameObject clone = Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Length)], triggeringObject.transform.position, Quaternion.identity);
        return true;
    }
}
