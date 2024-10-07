using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroSceneLoader : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.StopSong("MainMenu");
        StartCoroutine(CheckVideoPlaying());     
    }


    [SerializeField] VideoPlayer video;



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
