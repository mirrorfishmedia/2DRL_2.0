using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundEffect")]
public class SoundEffect : ScriptableObject
{
    public AudioClip[] clips;
    public float pitchCenter = 1f;
    public float pitchVariance = .15f;
    public float ampCenter = .5f;
    public float ampVariance = .15f;


}
