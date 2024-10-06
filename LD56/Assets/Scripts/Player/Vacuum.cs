using JigglePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    [SerializeField] Transform gunPosition;
    [SerializeField] Transform suckOrigin;

    [SerializeField] Transform suckCollider;
    [SerializeField] LayerMask suckable;

    [SerializeField] float suckStrength = 25f;

    [SerializeField] float maxPowerTime = 3f;
    [SerializeField] float maxAmplitude = .04f;
    [SerializeField] float minAmplitude = .005f;
    [SerializeField] float maxDistFromOrigin = 5f;
    [SerializeField] float minDistFromOrigin = .5f;
    [SerializeField] float maxSuckTime = 5f;
    [SerializeField] float chargeRate = 8f;
    private float totalSuckTime = 0;
    private float currentSuckTime = 0;




    [SerializeField] GameObject slimeTrail;
    [SerializeField] float slimeTraileWaitTime;
    HashSet<GameObject> activeSlimeParticles = new();
    float timeSinceLastSlimeTrail = 0;



    [SerializeField] float suckStrengthVisual = 300f;
    private Dictionary<Goo, float> selectedGoos = new();

    private VacuumParticles particles;
    private bool sucking = false;

    private void OnEnable()
    {
        totalSuckTime = maxSuckTime;
        particles = GetComponentInChildren<VacuumParticles>();

        particles.Amplitude = maxAmplitude;
        particles.DistFromOrigin = maxDistFromOrigin;

        particles.Deactivate();
    }

    void Update()
    {
        
        transform.position = gunPosition.position;
        transform.rotation = gunPosition.rotation;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            totalSuckTime += Time.deltaTime;
            totalSuckTime = Mathf.Clamp(totalSuckTime, 0, maxPowerTime);


            playerMovement.SetSpeedMultiplier(.6f);

            // The particles keep track of whether or not they're active so no need to do that here
            // Although I guess we could
            particles.Activate();
            sucking = true;
        }
        else
        {
            totalSuckTime -= Time.deltaTime;
            totalSuckTime = Mathf.Clamp(totalSuckTime, 0, Mathf.Infinity);


            playerMovement.SetSpeedMultiplier(1f);

            currentSuckTime += Time.deltaTime * chargeRate;
            currentSuckTime = Mathf.Clamp(currentSuckTime, 0, maxSuckTime);

            sucking = false;
            particles.Deactivate();
            AudioManager.instance.StopSound("VacuumHigh");
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AudioManager.instance.PlaySound("VacuumOn");
            AudioManager.instance.PlaySound("VacuumHigh");
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            AudioManager.instance.PlaySound("VacuumOff");
            AudioManager.instance.StopSound("VacuumHigh");
        }


        if (totalSuckTime >= maxPowerTime)
        {

            currentSuckTime -= Time.deltaTime;
            currentSuckTime = Mathf.Clamp(currentSuckTime, 0, maxSuckTime);

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
                Vector3 windDir;
                float forceMultiplier = 1f;

                if(other.TryGetComponent<Goo>(out Goo goo))
                {
                    if (selectedGoos.ContainsKey(goo))
                    {
                        selectedGoos[goo] += Time.deltaTime;
                        forceMultiplier = selectedGoos[goo];
                        if(forceMultiplier >= goo.CaptureTime)
                        {
                            goo.Capture(suckOrigin);
                        }

                        windDir = -(goo.transform.position - suckOrigin.position).normalized;

                        goo.GetComponent<JiggleRigBuilder>().wind = windDir * suckStrengthVisual;

                    }
                    else
                    {
                        selectedGoos.Add(goo, 0.0f);
                    }


                    timeSinceLastSlimeTrail += Time.deltaTime;

                    if(timeSinceLastSlimeTrail > slimeTraileWaitTime)
                    {
                        timeSinceLastSlimeTrail = 0f;
                        activeSlimeParticles.Add(Instantiate(slimeTrail, goo.transform.position + Random.insideUnitSphere, Quaternion.identity));
                    }

                    List<GameObject> itemsToRemove = new List<GameObject>();

                    foreach(GameObject slimeParticle in activeSlimeParticles)
                    {
                        slimeParticle.transform.position = Vector3.MoveTowards(slimeParticle.transform.position, suckOrigin.position, Time.deltaTime * 10f);

                        float dist = Vector3.Distance(transform.position, slimeParticle.transform.position);
                        if(dist <= .1)
                        {
                            itemsToRemove.Add(slimeParticle);
                        }
                    }

                    foreach(GameObject slimeParticle in itemsToRemove)
                    {
                        activeSlimeParticles.Remove(slimeParticle);
                        Destroy(slimeParticle);
                    }

                    goo.SetSpeedMultiplier(.5f);

                    goo.Select();

                    windDir = -(goo.transform.position - suckOrigin.position).normalized;

                    goo.GetComponent<JiggleRigBuilder>().wind = windDir * suckStrengthVisual;
                }


                Rigidbody otherRb = other.GetComponentInChildren<Rigidbody>();
                Vector3 force = -transform.right * Time.fixedDeltaTime * suckStrength * forceMultiplier;
                //Debug.Log(force);

                if(otherRb != null)
                {
                    otherRb.AddForce(force, ForceMode.Impulse);
                }
            }
            else
            {
                if (other.TryGetComponent<Goo>(out Goo goo))
                {
                    selectedGoos.Remove(goo);

                    goo.Deselect();
                    goo.SetSpeedMultiplier(1f);

                    Vector3 windDir = -(goo.transform.position - suckOrigin.position).normalized;

                    goo.GetComponent<JiggleRigBuilder>().wind = Vector3.zero;

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
                    goo.SetSpeedMultiplier(1f);


                    Vector3 windDir = -(goo.transform.position - suckOrigin.position).normalized;

                    goo.GetComponent<JiggleRigBuilder>().wind = Vector3.zero;
                }
            }
        }
    }
}
