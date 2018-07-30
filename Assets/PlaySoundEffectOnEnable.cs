using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffectOnEnable : MonoBehaviour {

    public SoundEffect soundEffect;

    TDR_GameMan gameMan;

    private void Awake()
    {
        gameMan = FindObjectOfType<TDR_GameMan>();

    }

    // Use this for initialization
    void OnEnable ()
    {
        gameMan.soundMan.PlaySoundEffect(soundEffect);

    }
}
