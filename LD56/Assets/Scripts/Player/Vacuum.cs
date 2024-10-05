using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Transform gunPosition;

    [SerializeField] Transform suckCollider;
    [SerializeField] LayerMask suckable;


    [SerializeField] float maxAmplitude = .04f;
    [SerializeField] float minAmplitude = .005f;
    [SerializeField] float maxDistFromOrigin = 5f;
    [SerializeField] float minDistFromOrigin = .5f;
    [SerializeField] float maxSuckTime = 5f;
    [SerializeField] float chargeRate = 8f;
    private float currentSuckTime = 0;

    private Dictionary<Goo, float> selectedGoos = new();

    private VacuumParticles particles;
    private bool sucking = false;


    int objectsInVaccumSuction = 0;

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
            playerMovement.SetSpeedMultiplier(.6f);

            currentSuckTime -= Time.deltaTime;
            currentSuckTime = Mathf.Clamp(currentSuckTime, 0, maxSuckTime);

            // The particles keep track of whether or not they're active so no need to do that here
            // Although I guess we could
            particles.Activate();
            sucking = true;
        }
        else
        {

            playerMovement.SetSpeedMultiplier(1f);

            currentSuckTime += Time.deltaTime * chargeRate;
            currentSuckTime = Mathf.Clamp(currentSuckTime, 0, maxSuckTime);

            sucking = false;
            particles.Deactivate();
        }


        particles.Amplitude = Mathf.Lerp(minAmplitude, maxAmplitude, currentSuckTime / maxSuckTime);
        particles.DistFromOrigin = Mathf.Lerp(minDistFromOrigin, maxDistFromOrigin, currentSuckTime / maxSuckTime);

        //Debug.Log(selectedGoos.Count);



        // We need to set the size of  the cone collider
        float xScale = particles.Amplitude * particles.NumCircles * Mathf.PI;
        float yScale = particles.Amplitude * particles.NumCircles * Mathf.PI;
        float zScale = particles.DistFromOrigin / 2f;
        suckCollider.transform.localScale = new Vector3(xScale, yScale, zScale);
    }

    private void OnTriggerStay(Collider other)
    {
        if((suckable & (1 << other.gameObject.layer)) != 0)
        {

            if (sucking)
            {
                float forceMultiplier = 1f;

                if(other.TryGetComponent<Goo>(out Goo goo))
                {
                    if (selectedGoos.ContainsKey(goo))
                    {
                        selectedGoos[goo] += Time.deltaTime;
                        forceMultiplier = selectedGoos[goo];
                    }
                    else
                    {
                        selectedGoos.Add(goo, 0.0f);
                    }

                    goo.Select();
                }


                Rigidbody otherRb = other.GetComponentInChildren<Rigidbody>();
                Vector3 force = -transform.right * Time.fixedDeltaTime * 25f * forceMultiplier;
                //Debug.Log(force);
                otherRb.AddForce(force, ForceMode.Impulse);
            }
            else
            {
                if (other.TryGetComponent<Goo>(out Goo goo))
                {
                    selectedGoos.Remove(goo);

                    goo.Deselect();
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sucking)
        {
            if ((suckable & (1 << other.gameObject.layer)) != 0)
            {
                if (other.transform.TryGetComponent<Goo>(out Goo goo))
                {
                    selectedGoos.Remove(goo);
                    goo.Deselect();
                }
            }
        }
    }
}
