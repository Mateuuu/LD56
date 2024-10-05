using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip soundClip;
    [Range(0, 1)] public float soundVolume;
    [Range(.1f, 3)] public float soundPitch;
    public bool isLooping;
    [HideInInspector] public AudioSource source;
}
