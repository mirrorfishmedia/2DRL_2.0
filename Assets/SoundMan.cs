using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMan : MonoBehaviour {

    public int numSources = 16;

    public AudioSource[] sources;

    // Use this for initialization
    void Awake()
    {
        sources = new AudioSource[numSources];
        for (int i = 0; i < numSources; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            sources[i] = source;
        }
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        for (int i = 0; i < numSources; i++)
        {
            if (!sources[i].isPlaying)
            {
                AudioSource source = sources[i];
                source.clip = soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
                source.pitch = soundEffect.pitchCenter + Random.Range(-soundEffect.pitchVariance, soundEffect.pitchVariance);
                source.volume = soundEffect.ampCenter + Random.Range(-soundEffect.ampVariance, soundEffect.ampVariance);
                source.Play();
                break;
            }
        }
    }
}
