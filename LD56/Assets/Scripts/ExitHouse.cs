using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHouse : MonoBehaviour
{

    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] Animator animator;

    bool ableToQuit;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {


            if(SceneManager.GetActiveScene().name == "Hehe")
            {
                animator.SetBool("Fade", true);

                ableToQuit = true;
                return;
            }


            if(GameManager.Instance.SlimesCaptured < GameManager.Instance.NumSlimes)
            {
                animator.SetBool("Fade", true);

                Debug.Log("Press enter to quit. You haven't caught all the slimes!");

                ableToQuit = true;


            }
            else
            {
                ableToQuit = false;
                sceneTransition.TransitionToScene("Win");
                Debug.Log("Yay you beat the game =D");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            animator.SetBool("Fade", false);
        }
    }

    private void Update()
    {
        if (ableToQuit)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneTransition.TransitionToScene("MainMenu");
            }
        }
    }

}
