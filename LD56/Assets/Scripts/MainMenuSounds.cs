using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSounds : MonoBehaviour
{
    IEnumerator Start()
    {
        AudioManager.instance.StopAllSongs();
        AudioManager.instance.StartSong("MainMenu");


        yield return new WaitForSeconds(.25f);
        AudioManager.instance.PlaySound("VanEnter");

        yield return new WaitForSeconds(6.6f);
        AudioManager.instance.PlaySound("VanIdle");

    }
}
