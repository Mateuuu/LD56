using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroSceneLoader : MonoBehaviour
{
    [SerializeField] VideoPlayer video;
    private void Start()
    {


        video.SetDirectAudioVolume(0, AudioManager.instance.sfxVolume);

        AudioManager.instance.StopSong("MainMenu");
        StartCoroutine(CheckVideoPlaying());     
    }





    IEnumerator CheckVideoPlaying()
    {
        yield return new WaitForSeconds(4f);



        while(true)
        {
            if(!video.isPlaying)
            {
                SceneManager.LoadScene("Hallway");
                break;
            }

            yield return null;
        }
    }
}
