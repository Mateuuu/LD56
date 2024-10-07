using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooJar : MonoBehaviour
{

    MeshRenderer renderer;
    Material material;


    private int slimesCaptured = 0;

    private void Start()
    {

        renderer = GetComponent<MeshRenderer>();
        material = renderer.materials[1];

    }

    private void Update()
    {
        material.SetFloat("_FillLevel", (float)GameManager.Instance.SlimesCaptured / GameManager.Instance.NumSlimes);
    }

}
