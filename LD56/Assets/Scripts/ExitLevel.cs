using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{

    [SerializeField] string sceneToLoad;

    [SerializeField] Vector3 playerDoorPoint;
    [SerializeField] Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {

            if (playerDoorPoint != null && playerTransform != null)
            {
                playerTransform.position = playerDoorPoint;
            }

            HouseSceneManager.Instance.SetSceneComingFrom(sceneToLoad);
        }
    }
}
