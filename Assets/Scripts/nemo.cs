using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions
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
        if (velocity.sqrMagnitude > 0.01f) // Ensure the shark is moving
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 180f; // Add 180 degrees to face the correct direction
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0, 0, angle), // Rotate to face the movement direction
                Time.deltaTime * 5f // Smooth rotation speed
            );

            // Flip the sprite if the angle is between 90 and 270 degrees
            if (angle > 90f && angle < 270f || angle < -90f && angle > -270f)
            {
                spriteRenderer.flipY = true; // Flip the sprite vertically
            }
            else
            {
                spriteRenderer.flipY = false; // Reset the sprite flip
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Transition to the win page
            SceneManager.LoadScene("WinScene"); // Replace "WinScene" with the name of your win scene
        }
    }
}