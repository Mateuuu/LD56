using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{

    VideoPlayer player;
    void Awake()
    {
        player = GetComponent<VideoPlayer>();
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Intro.mp4");

    }

    private void Start()
    {
        player.Play();
    }

}
