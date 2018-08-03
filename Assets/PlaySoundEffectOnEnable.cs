using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffectOnEnable : MonoBehaviour {

    public SoundEffect soundEffect;

    // Use this for initialization
    void OnEnable ()
    {
        GameMan.gm.soundMan.PlaySoundEffect(soundEffect);
    }
}
