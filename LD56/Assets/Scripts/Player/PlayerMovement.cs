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

    Animator animator;

    private float currSpeedMultiplier = 1f;

    Rigidbody rb;
    Vector2 movement;

    Camera cam;


    AudioSource footstepAudio;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        animator.speed = 2f;

        cam = Camera.main;
        rb = GetComponent<Rigidbody>();    
    }

    private void Start()
    {
        footstepAudio = AudioManager.instance.GetSoundSource("Footstep");
    }

    float amountForward = 0f;

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

        animator.SetFloat("MoveSpeed", movement.normalized.magnitude);

        amountForward = Vector3.Dot(transform.forward, new Vector3(movement.x, 0, movement.y));

        animator.SetFloat("DotProduct", amountForward);
            



        // movement.x = Mathf.Pow(movement.x, 5);          // Make sure this is an odd power so that the sign stays consistent
        // movement.y = Mathf.Pow(movement.y, 5);
    }



    [SerializeField] int targetFootStepRate = 8;
    [SerializeField] float minFootstepPitch = .8f;
    [SerializeField] float maxFootstepPitch = 1.2f;
    int i = 0;
    private void FixedUpdate()
    {

        float newFootstepRate = targetFootStepRate;
        if (Mathf.Abs(amountForward) > .5f)
        {
            newFootstepRate = (int)(targetFootStepRate / 2);
        }

        if (movement.sqrMagnitude > 0 && i % newFootstepRate == 0)
        {
            footstepAudio.pitch = Random.Range(minFootstepPitch, maxFootstepPitch);
            AudioManager.instance.PlaySound("Footstep");
        }
        i++;

        float moveMultiplier = 1f;

        if(movement.x != 0 && movement.y != 0)
        {
            moveMultiplier *= .7071f;           // Sqrt(2) / 2
        }


        float xMovement = movement.x * moveSpeed * moveMultiplier * currSpeedMultiplier;
        float yMovement = movement.y * moveSpeed * moveMultiplier * currSpeedMultiplier;

        Vector3 targetVelocity = new Vector3(xMovement, 0, yMovement);

        rb.velocity = new Vector3(xMovement, 0, yMovement);

        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        currSpeedMultiplier = speedMultiplier;
    }

}
