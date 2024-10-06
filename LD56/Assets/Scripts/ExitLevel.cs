using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{

    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {

            string currScene = SceneManager.GetActiveScene().name;

            if (currScene == "Hallway")
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                HouseSceneManager.Instance.SetSceneComingFrom(currScene);
                SceneManager.LoadScene("Hallway");
            }
        }
    }

}
