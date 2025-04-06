using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nemo : Enemy
{
    public float swimSpeed = 2f; // Speed at which Nemo swims away
    public float safeDistance = 10f; // Distance at which Nemo starts swimming away

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isMoving = false;

    public new void Start()
    {
        base.Start(); // Call the base class Start method

        // Initialize components
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Set NavMeshAgent properties
        agent.speed = swimSpeed;
    }

    public new void Update()
    {
        base.Update(); // Call the base class Update method

        if (!isDead && player != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // If the player is within the safe distance, Nemo swims away
            if (distanceToPlayer <= safeDistance)
            {
                // Calculate the direction away from the player
                Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

                // Determine a target position further away from the player
                Vector3 targetPosition = transform.position + directionAwayFromPlayer * 5f;

                // Set the NavMeshAgent's destination to the target position
                agent.SetDestination(targetPosition);

                // Check if Nemo is moving
                isMoving = agent.velocity.magnitude > 0.1f;

                // Rotate Nemo to face the movement direction
                RotateToFaceMovement(agent.velocity);

                // Play the swim animation if Nemo is moving
                if (isMoving)
                {
                    animator.Play("Swim");
                }
            }
            else
            {
                // Stop moving if the player is outside the safe distance
                agent.ResetPath();
                isMoving = false;
            }

            // Constrain Nemo's Z position to -1 to avoid rendering issues
            Vector3 constrainedPosition = transform.position;
            transform.position = constrainedPosition;
        }
    }

    private void RotateToFaceMovement(Vector3 velocity)
    {
        if (velocity.x > 0) // Moving right
        {
            spriteRenderer.flipX = true; // Flip the sprite horizontally
        }
        else if (velocity.x < 0) // Moving left
        {
            spriteRenderer.flipX = false; // Reset the sprite to default orientation
        }
    }
}