using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneManager : MonoBehaviour
{

    // Stores a list of positions and strings, where the string is the scene name and the position is where to spawn to
    Dictionary<string, Vector3> sceneToPosition = new();

    [SerializeField] List<string> sceneNames;
    [SerializeField] List<Vector3> locations;

    [SerializeField] Transform spawnPoint;

    Vector3 newPlayerPos;

    public static HouseSceneManager Instance;
    private void Awake()
    {

        if(spawnPoint != null)
        {
            newPlayerPos = spawnPoint.position;
        }

        for(int i = 0; i < sceneNames.Count; i++)
        {
            sceneToPosition[sceneNames[i]] = locations[i];
        }


        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }



    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;

        if(scene.name == "Hallway")
        {
            Debug.Log("moving player to: " + newPlayerPos);
            playerTransform.position = newPlayerPos;
        }



    }

    public void SetSceneComingFrom(string sceneName)
    {
        newPlayerPos = sceneToPosition[sceneName];
        Debug.Log(newPlayerPos);
    }
}
