using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nemo : MonoBehaviour
{
    public float swimSpeed = 2f; // Speed at which Nemo swims away
    public float safeDistance = 10f; // Distance at which Nemo starts swimming away
    public Transform player;    // Reference to the player's transform
    private NavMeshAgent navMeshAgent;
    private Animator animator;  // Reference to the Animator component
    private bool isMoving = false; // Track if Nemo is moving

    void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = swimSpeed; // Set the swim speed

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Only swim away if the player is within the safe distance
            if (distanceToPlayer <= safeDistance)
            {
                // Calculate the direction away from the player
                Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;

                // Determine a target position further away from the player
                Vector3 targetPosition = transform.position + directionAwayFromPlayer * 5f;

                // Set the NavMeshAgent's destination to the target position
                navMeshAgent.SetDestination(targetPosition);

                // Set isMoving to true if Nemo is moving
                isMoving = navMeshAgent.velocity.magnitude > 0.1f;
            }
            else
            {
                // Stop moving if the player is outside the safe distance
                navMeshAgent.ResetPath();
                isMoving = false;
            }

            // Update the animation state
            animator.SetBool("IsMoving", isMoving);
        }
    }
}