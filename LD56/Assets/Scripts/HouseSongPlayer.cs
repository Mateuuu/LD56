using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSongPlayer : MonoBehaviour
{
    IEnumerator Start()
    {

        AudioManager.instance.StartSong("HouseOpening");
        AudioClip clip = AudioManager.instance.GetSongClip("HouseOpening");

        WaitForSeconds wait = new WaitForSeconds(clip.length);

        yield return wait;

        AudioManager.instance.StartSong("HouseLoop");
    }

}
