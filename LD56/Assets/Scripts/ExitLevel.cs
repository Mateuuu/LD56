using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{

    [SerializeField]
    [SceneAttribute] string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {

            SceneManager.LoadScene(sceneToLoad);

        }
    }

}
