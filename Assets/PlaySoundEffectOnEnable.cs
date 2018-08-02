using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffectOnEnable : MonoBehaviour {

    public SoundEffect soundEffect;

    GameMan gameMan;

    private void Awake()
    {
        gameMan = FindObjectOfType<GameMan>();

    }

    // Use this for initialization
    void OnEnable ()
    {
        gameMan.soundMan.PlaySoundEffect(soundEffect);

    }
}
