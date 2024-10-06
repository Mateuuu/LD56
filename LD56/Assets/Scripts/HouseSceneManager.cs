using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseSceneManager : MonoBehaviour
{

    // Stores a list of positions and strings, where the string is the scene name and the position is where to spawn to
    Dictionary<string, Vector3> sceneToPosition = new();
    Dictionary<string, GameObject> sceneToScenePrefab = new();

    [SerializeField] List<string> sceneNames;
    [SerializeField] List<Vector3> locations;
    [SerializeField] List<GameObject> scenePrefabs;

    [SerializeField] Transform spawnPoint;

    Vector3 newPlayerPos;

    [SerializeField] GameObject hallwayScene;
    GameObject activeScene;
    string activeSceneName;

    public static HouseSceneManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(spawnPoint != null)
        {
            newPlayerPos = spawnPoint.position;
            activeScene = hallwayScene;
        }

        for(int i = 0; i < sceneNames.Count; i++)
        {
            sceneToPosition[sceneNames[i]] = locations[i];
            sceneToScenePrefab[sceneNames[i]] = scenePrefabs[i];
        }
    }

    public void SetSceneComingFrom(string sceneToLoad)
    {


        // null string means we want to go to the hallway
        if(sceneToLoad == "")
        {
            Debug.Log("Loading to hallway, from: " + activeSceneName);
            activeScene.SetActive(false);
            hallwayScene.SetActive(true);

            Transform playerTransform = FindObjectOfType<PlayerMovement>().transform;
            newPlayerPos = sceneToPosition[activeSceneName];
            playerTransform.position = newPlayerPos;

            activeScene = hallwayScene;

        }
        else
        {
            Debug.Log("Loading to " + sceneToLoad + ", from: " + activeSceneName);


            GameObject newScene = sceneToScenePrefab[sceneToLoad];
            activeScene.SetActive(false);
            newScene.SetActive(true);
            activeScene = newScene;
        }

        activeSceneName = sceneToLoad;
    }
}
