using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] Animator vanAnimator;

    [SerializeField] SceneTransition sceneTransition;
    public void Play()
    {
        AudioManager.instance.PlaySound("ButtonClick");

        StartCoroutine(PlayTransition());

    }

    IEnumerator PlayTransition()
    {

        vanAnimator.SetBool("DriveAway", true);
        AudioManager.instance.PlaySound("VanDrive");


        yield return new WaitForSeconds(1f);

        // Do a coroutine here that makes it so that the car drives away with some sounds =D

        sceneTransition.TransitionToScene("Intro Scene");
    }


    public void Quit()
    {
        AudioManager.instance.PlaySound("ButtonClick");

        Application.Quit();
    }

    public void Hehe()
    {
        AudioManager.instance.PlaySound("ButtonClick");

        sceneTransition.TransitionToScene("Hehe");
    }

    public void Menu()
    {
        AudioManager.instance.PlaySound("ButtonClick");

        sceneTransition.TransitionToScene("MainMenu");
    }
}
