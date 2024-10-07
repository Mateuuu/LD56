using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.StopAllSongs();
        AudioManager.instance.StartSong("MainMenu");
    }
}
