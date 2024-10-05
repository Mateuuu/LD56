using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumParticles : MonoBehaviour
{
    [SerializeField] private int numParticles = 7;
    [SerializeField] private GameObject particlePrefab;

    [SerializeField] private int numCircles = 15;
    [SerializeField] private float amplitude = .05f;
    [SerializeField] private float distFromOrigin = 4f;
    [SerializeField] private float speed = 20f;

    List<TrailRenderer> particles = new();

    private bool active = false;

    [HideInInspector]
    public float DistFromOrigin
    {
        get
        {
            return distFromOrigin;
        }
        set {
            distFromOrigin = value;
        }
    }
    [HideInInspector]
    public float Amplitude
    {
        get
        {
            return amplitude;
        }
        set {
            amplitude = value;
        }
    }
    [HideInInspector]
    public float NumCircles
    {
        get
        {
            return numCircles;
        }
        private set { }
    }


    private void Awake()
    {
        for(int i = 0; i < numParticles; i++)
        {
            particles.Add(GameObject.Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<TrailRenderer>());
        }
    }

    // We're just gonna have a constant function running in update that updates the locations of the particles.
    float t = 0;
    private void Update()
    {

        float origT = t;

        float offset = (numCircles * Mathf.PI) / numParticles;
        foreach(TrailRenderer particle in particles)
        {
            t = t % (numCircles * Mathf.PI);

            float xPos = (amplitude * (numCircles * Mathf.PI - t) * Mathf.Cos(t));
            float yPos = (amplitude * (numCircles * Mathf.PI - t) * Mathf.Sin(t));
            float zPos = ((distFromOrigin / (numCircles * Mathf.PI) * (numCircles * Mathf.PI - t)));

            particle.transform.localPosition = new Vector3(xPos, yPos, zPos);

            t += offset;
        }

        // Preserve t, as we screw with it in the loop
        t = origT;

        t += Time.deltaTime * speed;

    }

    public void Activate()
    {

        if (!active)
        {
            // Do the fade in animation of the particles that are 

            foreach(TrailRenderer particle in particles)
            {
                particle.enabled = true;
                //particle.Play();    
            }
        }

        active = true;
    }

    public void Deactivate()
    {

        if (active)
        {
            foreach (TrailRenderer particle in particles)
            {
                particle.enabled = false;
                //particle.Stop();
            }
        }

        active = false;
    }
}
