using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1BitRogue/Effects/SpawnObject")]

public class SpawnObjectEffect : GameEffect {

    public GameObject[] objectsToSpawn;

    public override void TriggerEffect(GameObject triggeringObject, GameObject triggeredObject)
    {
        //Debug.Log("spawn object effect, triggering " + triggeringObject.name);
        GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        GameObject clone = Instantiate(randomObject, triggeringObject.transform.position, Quaternion.identity);
        //Debug.Log("<color=yellow> chose and spawned randomObject</color> " + clone.name);

    }
}
