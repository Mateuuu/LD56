using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{

    [SerializeField] Transform gunPosition;

    [SerializeField] Transform suckCollider;
    [SerializeField] LayerMask suckable;

    private VacuumParticles particles;
    private bool sucking = false;

    private void Awake()
    {
        particles = GetComponentInChildren<VacuumParticles>();
        particles.Deactivate();
    }

    void Update()
    {
        
        transform.position = gunPosition.position;
        transform.rotation = gunPosition.rotation;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // The particles keep track of whether or not they're active so no need to do that here
            // Although I guess we could
            particles.Activate();
            sucking = true;
        }
        else
        {
            sucking = false;
            particles.Deactivate();
        }



        // We need to set the size of  the cone collider
        float xScale = particles.Amplitude * particles.NumCircles * Mathf.PI;
        float yScale = particles.Amplitude * particles.NumCircles * Mathf.PI;
        float zScale = particles.DistFromOrigin / 2f;
        suckCollider.transform.localScale = new Vector3(xScale, yScale, zScale);

    }

    private void OnTriggerStay(Collider other)
    {
        if(sucking)
        {
            if((suckable & (1 << other.gameObject.layer)) != 0)
            {
                Rigidbody otherRb = other.GetComponentInChildren<Rigidbody>();
                Vector3 force = -transform.right * Time.fixedDeltaTime * 25f;
                //Debug.Log(force);
                otherRb.AddForce(force, ForceMode.Impulse);
                //otherRb.velocity = Vector3.Lerp(otherRb.velocity, force, Time.fixedDeltaTime);
            }
        }
    }
}
