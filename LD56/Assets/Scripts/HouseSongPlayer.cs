using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSongPlayer : MonoBehaviour
{
    IEnumerator Start()
    {
        AudioManager.instance.StartSong("HouseOpening");
        AudioSource source = AudioManager.instance.GetSongSource("HouseOpening");


        while (true)
        {
            if (!source.isPlaying)
            {
                AudioManager.instance.StartSong("HouseLoop");
                break;
            }
            yield return null;
        }
    }

}
