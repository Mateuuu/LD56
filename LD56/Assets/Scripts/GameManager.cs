using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private int slimesCaptured = 0;
    public int SlimesCaptured
    {
        get
        {
            return slimesCaptured;
        }
        private set { }
    }


    [SerializeField] private int numSlimes = 5;

    public int NumSlimes
    {
        get
        {
            return numSlimes;
        }
        private set { }
    }



    public event Action<int> slimeCaptured;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void SlimeCaptured()
    {
        slimesCaptured++;
        slimeCaptured?.Invoke(slimesCaptured);
    }
    
}
