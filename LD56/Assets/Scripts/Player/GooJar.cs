using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooJar : MonoBehaviour
{

    MeshRenderer renderer;
    Material material;

    private void Start()
    {

        renderer = GetComponent<MeshRenderer>();
        material = renderer.materials[1];

        GameManager.Instance.slimeCaptured += UpdateJarLevel;

        UpdateJarLevel(GameManager.Instance.SlimesCaptured);
    }

    private void OnDisable()
    {
        GameManager.Instance.slimeCaptured -= UpdateJarLevel;

    }


    void UpdateJarLevel(int slimesCaptured)
    {

        material.SetFloat("_FillLevel", (float) slimesCaptured / GameManager.Instance.NumSlimes);

    }

}
