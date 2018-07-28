using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDR_PlayerPositioner : MonoBehaviour 
{
	Transform playerTransform;

    private void OnEnable()
    {
        playerTransform = GameObject.FindObjectOfType<PlayerIdentifier>().transform;
        playerTransform.position = this.transform.position;
    }
}
