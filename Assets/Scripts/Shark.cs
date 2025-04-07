using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Shark : Enemy
{
    public float chompCooldown = 1f;
    public float distToChomp = 3f;
    public float chompDamage = 25f;
    public CircleCollider2D chomper;

    public Transform leftChompPoint;
    public Transform rightChompPoint;

    Animator animator;
    SpriteRenderer spriteRenderer;
    SpriteRenderer chompRenderer;
    Blinker blinker;
    float lastChompTime = -Mathf.Infinity;
    bool isBloody = false;
    bool isMoving = false;

    public new void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        chompRenderer = chomper.gameObject.GetComponent<SpriteRenderer>();
        blinker = gameObject.AddComponent<Blinker>(); 

        chomper.isTrigger = true; 
        chompRenderer.enabled = false; 
    }

    public new void Update()
    {
        base.Update();

        // Check if the shark is moving
        var navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        isMoving = navMeshAgent.velocity.magnitude > 0.1f;

        // Rotate the shark to face the movement direction
        RotateToFaceMovement(navMeshAgent.velocity);

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= distToChomp && Time.time - lastChompTime >= chompCooldown)
        {
            Chomp();
            lastChompTime = Time.time;
        }
    }

    void Chomp() {
        if (health < 0) return;
        if (health < 51) {
            animator.Play("Chomp Bloody");
        } else {
            animator.Play("Chomp");
        }

        if (chomper.IsTouching(player.gameObject.GetComponent<Collider2D>())) {
            player.TakeDamage(chompDamage);
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
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         // Handle player collision if needed
    //         // For example, apply damage or effects
    //     }

    //     if (collision.gameObject.CompareTag("Projectile"))
    //     {
    //         StartCoroutine(blinker.blinkFor(0.5f));
    //     }
    // }

    public override void TakeDamage(float damage)
    {
        if (isDead)
        {
            return; // Don't do anything if already dead
        }

        base.TakeDamage(damage); // Reduce health by the damage amount

        StartCoroutine(blinker.blinkFor(1f)); // Visual feedback for taking damage

        if (health <= 0)
        {
            isDead = true; // Mark the shark as dead
            Die(); // Trigger death logic
        }
        else if (health < 51 && !isBloody)
        {
            isBloody = true;
            animator.Play("Swim Bloody"); // Play bloody swim animation
        }
    }


    private void Die()
    {
        Destroy(gameObject, 0.1f); // Destroy the shark after 1 second to allow the animation to play
    }
}