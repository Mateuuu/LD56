using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Goo : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float enemyDistanceRun = 5f;

    [SerializeField] ParticleSystem deathParticle;

    [SerializeField] float baseSpeed = 10f;

    Collider collider;

    private NavMeshAgent agent;

    private bool captured = false;

    private Transform captureTarget;
    [SerializeField] private float captureMoveSpeed = 5f;

    [SerializeField] private float captureTime = 4f;

    private float speedMultiplier = 1f;
    public float CaptureTime
    {
        get
        {
            return captureTime;
        }
        set
        {
            captureTime = value;
        }
    }

    [SerializeField] SkinnedMeshRenderer meshRenderer;


    [SerializeField] List<string> gooCaptureSounds;

    Material material;
    void Awake()
    {
        collider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        material = meshRenderer.material;
    }

    void Update()
    {

        agent.speed = baseSpeed * speedMultiplier;

        if (captured)
        {
            transform.position = Vector3.MoveTowards(transform.position, captureTarget.position, Time.deltaTime * captureMoveSpeed);

            float dist = Vector3.Distance(captureTarget.position, transform.position);

            if(dist < .05f)
            {
                Instantiate(deathParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);

                AudioManager.instance.PlaySound(gooCaptureSounds[Random.Range(0, gooCaptureSounds.Count)]);
                AudioManager.instance.PlaySound("GooSucked");

                GameManager.Instance.SlimeCaptured();

            }
        }
        else
        {
            Vector3 runTo = transform.position + ((transform.position - player.position));
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < enemyDistanceRun)
            {
                FleeToFarthestPoint();
            }
            else
            {
                Wander(1f, .25f);
            }
        }

    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public void Capture(Transform target)
    {
        captured = true;
        captureTarget = target;
        collider.enabled = false;
        agent.enabled = false;
    }

    public void Select()
    {
        material.SetFloat("_Size", .2f);
    }

    public void Deselect()
    {
        material.SetFloat("_Size", .0f);
    }


    void FleeToFarthestPoint()
    {
        float searchRadius = 20f; // Radius in which to search for points on the NavMesh
        int maxAttempts = 15; // Number of random points to evaluate
        Vector3 bestFleeTarget = Vector3.zero;
        float bestDistance = 0f;

        // Try a series of random points within the search radius
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random point in the search radius
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
            {
                // Calculate distance from player to the potential flee point
                float distanceFromPlayer = Vector3.Distance(hit.position, player.position);

                // If this point is farther than the current best, use it
                if (distanceFromPlayer > bestDistance)
                {
                    bestDistance = distanceFromPlayer;
                    bestFleeTarget = hit.position;
                }
            }
        }

        // If we found a valid target, set it as the destination
        if (bestFleeTarget != Vector3.zero)
        {
            agent.SetDestination(bestFleeTarget);
        }
    }


    // Timer to track when to choose a new wander target
    private float wanderTimer = 0f;

    void Wander(float wanderRadius, float wanderCooldown)
    {
        // Update the timer
        wanderTimer += Time.deltaTime;

        // If the timer exceeds the cooldown, choose a new random target
        if (wanderTimer >= wanderCooldown)
        {
            Vector3 wanderTarget = GetRandomPointOnNavMesh(wanderRadius);
            if (wanderTarget != Vector3.zero)
            {
                agent.SetDestination(wanderTarget);
            }

            // Reset the timer after setting the new destination
            wanderTimer = 0f;
        }
    }

    // Function to find a random point on the NavMesh within the specified radius
    Vector3 GetRandomPointOnNavMesh(float radius)
    {
        Vector3 randomDirection = Random.onUnitSphere * radius; // Generate a random direction
        randomDirection += transform.position; // Offset by the enemy's current position

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas))
        {
            return navHit.position; // Return the valid point on the NavMesh
        }

        return Vector3.zero; // Return zero if no valid point is found
    }

}
