using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public static AudioManager instance;
    [SerializeField] Sound[] sounds;
    [SerializeField] Song[] songs;
    [SerializeField]
    AudioMixerGroup _mixer;

    void Awake()
    {
        sfxVolume = 1f;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.volume = s.soundVolume;
            s.source.pitch = s.soundPitch;
            s.source.loop = s.isLooping;
            s.source.outputAudioMixerGroup = _mixer;
        }
        foreach (Song s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.songClip;
            s.source.volume = s.songVolume;
            s.source.pitch = s.songPitch;
            s.source.loop = s.isLooping;
            s.source.outputAudioMixerGroup = _mixer;
        }
    }

    #region Sounds
    public void PlaySound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }
        s.source.Play();
    }
    public void StopSound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }
        s.source.Stop();
    }
    #endregion
    #region Songs
    public void StartSong(string songName)
    {
        Song s = Array.Find(songs, song => song.songName == songName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + songName + " not found.");
            return;
        }
        s.source.Play();
    }
    public void StopSong(string songName)
    {
        Song s = Array.Find(songs, song => song.songName == songName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + songName + " not found.");
            return;
        }
        s.source.Stop();
    }
    #endregion
}