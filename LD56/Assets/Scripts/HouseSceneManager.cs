using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSceneManager : MonoBehaviour
{

    // Stores a list of positions and strings, where the string is the scene name and the position is where to spawn to
    [SerializeField] 
    Dictionary<string, Transform> sceneToPosition = new();

    public static HouseSceneManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
