using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayCamera : MonoBehaviour
{
    [SerializeField] Transform playerTransform;


    [SerializeField] float minCameraZ = 0f;

    [SerializeField] float minCameraX = 0f;
    [SerializeField] float maxCameraX = 0f;


    Vector3 cameraOffset;

    private void Awake()
    {
        cameraOffset = transform.position;
    }
    void Update()
    {
        Vector3 targetPos = playerTransform.position + cameraOffset;

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraX, maxCameraX);
        targetPos.z = Mathf.Clamp(targetPos.z, minCameraZ, Mathf.Infinity);
        targetPos.y = cameraOffset.y;
        transform.position = targetPos;
    }
}
