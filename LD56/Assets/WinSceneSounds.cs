using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSceneSounds : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.StopAllSongs();
        AudioManager.instance.StartSong("Win");
    }
}
