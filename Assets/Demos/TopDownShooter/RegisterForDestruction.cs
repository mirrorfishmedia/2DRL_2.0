using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterForDestruction : MonoBehaviour {

    private void OnEnable()
    {
        GameMan.gm.ScheduleForDestruction(this.gameObject);
    }
}
