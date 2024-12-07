using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {
    public Transform player;                // Reference to the player's Transform
    public Transform ghostModel;            // Reference to the ghost model Transform (child)
    public float detectionRange = 40f;      // Range within which the ghost can detect the player
    public float followSpeed = 4.5f;        // Speed of the ghost when following
    public float fieldOfView = 360f;        // Full 360-degree field of view
    public float roamRadius = 15f;          // Radius within which the ghost will roam
    public float roamWaitTime = 1f;         // Wait time at each roam point
    public float followDelay = 8f;          // Time the ghost continues to follow the player after losing sight
    public int numberOfRays = 36;           // Number of rays to cast in 360 degrees

    private NavMeshAgent agent;
    private bool playerInSight;
    private Vector3 roamPoint;
    private bool isRoaming;
    private float roamWaitTimer;
    private float followTimer;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = followSpeed;
        agent.updateRotation = true; // Enable NavMeshAgent automatic rotation
        StartRoaming(); // Start roaming from the beginning
    }

    void Update() {
        DetectPlayer();

        if (playerInSight || followTimer > 0) {
            FollowPlayer();
            if (!playerInSight) {
                followTimer -= Time.deltaTime; // Decrease follow timer when player is not in sight
            }
        } else {
            if (isRoaming) {
                Roam();
            } else {
                StartRoaming();
            }
        }

        RotateModelTowardsMovement();
    }

    void DetectPlayer() {
        // Full 360-degree raycast detection
        float angleIncrement = 360f / numberOfRays;
        playerInSight = false; // Reset player sight detection for this frame

        for (int i = 0; i < numberOfRays; i++) {
            // Calculate the direction for each ray
            float angle = i * angleIncrement;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Cast a ray in the calculated direction
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange)) {
                // Check if the raycast hit the player
                if (hit.transform == player) {
                    playerInSight = true;
                    isRoaming = false; // Stop roaming if player is in sight
                    followTimer = followDelay; // Reset the follow timer
                    Debug.Log("Player in sight, following.");
                    return; // Stop checking further rays if player is detected
                }
            }
        }
    }

    void FollowPlayer() {
        // Set the player's position as the destination for the ghost
        agent.SetDestination(player.position);
    }

    void Roam() {
        // Check if the ghost has reached the roam point
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            roamWaitTimer += Time.deltaTime;
            if (roamWaitTimer >= roamWaitTime) {
                SetNewRoamPoint();
                roamWaitTimer = 0f;
                Debug.Log("Moving to new roam point.");
            }
        } else {
            // Keep the ghost moving to the roam point
            agent.SetDestination(roamPoint);
        }
    }

    void StartRoaming() {
        isRoaming = true;
        SetNewRoamPoint();
        Debug.Log("Starting roaming.");
    }

    void SetNewRoamPoint() {
        // Choose a random point within the roam radius
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;

        // Check if the random point is on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out navHit, roamRadius, NavMesh.AllAreas)) {
            roamPoint = navHit.position;
            agent.SetDestination(roamPoint);
            Debug.Log("New roam point set.");
        }
    }

    void RotateModelTowardsMovement() {
        // Rotate the ghost model towards the movement direction if the agent is moving
        if (agent.velocity.sqrMagnitude > 0.1f) // Check if there's significant movement
        {
            Quaternion lookRotation = Quaternion.LookRotation(agent.velocity.normalized);
            ghostModel.rotation = Quaternion.Slerp(ghostModel.rotation, lookRotation, Time.deltaTime * followSpeed);
        }
    }
}
