using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] LayerMask floor;
    [SerializeField] float _aimSpeed = 540;

    [SerializeField] Transform crosshair;

    Rigidbody rb;
    Vector2 movement;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100, floor)){

            crosshair.position = hit.point;
        }

        Vector3 lookVector = crosshair.position - transform.position;
        lookVector.y = 0;
        Quaternion lookRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookVector, Vector3.up), _aimSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(lookRotation);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // movement.x = Mathf.Pow(movement.x, 5);          // Make sure this is an odd power so that the sign stays consistent
        // movement.y = Mathf.Pow(movement.y, 5);
    }


    private void FixedUpdate()
    {
        float moveMultiplier = 1f;

        if(movement.x != 0 && movement.y != 0)
        {
            moveMultiplier *= .7071f;           // Sqrt(2) / 2
        }


        float xMovement = movement.x * moveSpeed * moveMultiplier;
        float yMovement = movement.y * moveSpeed * moveMultiplier;

        Vector3 targetVelocity = new Vector3(xMovement, 0, yMovement);

        rb.velocity = new Vector3(xMovement, 0, yMovement);

        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

}
