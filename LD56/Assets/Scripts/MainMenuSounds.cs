using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.StartSong("MainMenu");
    }
}
