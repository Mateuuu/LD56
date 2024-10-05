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

    List<ParticleSystem> particles = new();

    private bool active = false;

    public float DistFromOrigin
    {
        get
        {
            return distFromOrigin;
        }
        private set {}
    }
    public float Amplitude
    {
        get
        {
            return amplitude;
        }
        private set { }
    }
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
            particles.Add(GameObject.Instantiate(particlePrefab, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>());
        }
    }

    // We're just gonna have a constant function running in update that updates the locations of the particles.
    float t = 0;
    private void Update()
    {

        float origT = t;

        float offset = (numCircles * Mathf.PI) / numParticles;
        foreach(ParticleSystem particle in particles)
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

            foreach(ParticleSystem particle in particles)
            {
                particle.Play();    
            }
        }

        active = true;
    }

    public void Deactivate()
    {

        if (active)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
        }

        active = false;
    }
}
