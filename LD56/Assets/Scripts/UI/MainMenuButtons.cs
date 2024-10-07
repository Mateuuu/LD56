using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] SceneTransition sceneTransition;
    public void Play()
    {

        // Do a coroutine here that makes it so that the car drives away with some sounds =D

        sceneTransition.TransitionToScene("Intro Scene");
    }

    public void Quit()
    {

        Application.Quit();
    }
}
