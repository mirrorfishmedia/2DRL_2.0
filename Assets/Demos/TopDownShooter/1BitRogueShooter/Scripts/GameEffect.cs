using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEffect : ScriptableObject {

    public abstract bool TriggerEffect(GameObject triggeringObject);
}
