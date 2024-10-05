using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Song
{
    public string songName;
    public AudioClip songClip;
    [Range(0, 1)] public float songVolume;
    [Range(.1f, 3)] public float songPitch;
    public bool isLooping;
    [HideInInspector] public AudioSource source;
}
