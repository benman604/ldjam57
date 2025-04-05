using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Enemy
{
    public int health = 50; // Shark starts with 50 HP
    public Sprite[] sprites; // Array to hold the 6 sprites
    private SpriteRenderer spriteRenderer;
    private bool isMoving = false; // Tracks if the shark is moving
    private int currentIdleIndex = 0; // Tracks the current idle sprite index
    public float chompRange = 2f; // Distance within which the shark chomps
    public float chompCooldown = 1.5f; // Cooldown time between chomps
    private bool canChomp = true; // Tracks if the shark can chomp

    // Start is called before the first frame update
    void Start()
    {
        base.Start(); // Call the base class Start method
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite to idle_1
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }

        // Start the coroutine to switch idle sprites
        StartCoroutine(SwitchIdleSprites());
    }

    // Update is called once per frame
    void Update()
    {
        base.Update(); // Call the base class Update method (handles NavMeshAgent movement)

        // Check if the shark is moving
        var navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        isMoving = navMeshAgent.velocity.magnitude > 0.1f;

        // Rotate the shark to face the movement direction
        RotateToFaceMovement(navMeshAgent.velocity);

        // Check if the shark is close enough to the player to chomp
        CheckForChomp();

        // Update sprite based on health
        UpdateSprite();
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

    private void UpdateSprite()
    {
        if (health > 25)
        {
            // Full health sprites
            if (spriteRenderer.sprite == sprites[2] || spriteRenderer.sprite == sprites[4])
            {
                spriteRenderer.sprite = sprites[0]; // Switch to idle_1
            }
        }
        else
        {
            // 50% health sprites
            if (spriteRenderer.sprite == sprites[0] || spriteRenderer.sprite == sprites[2])
            {
                spriteRenderer.sprite = sprites[1]; // Switch to idle_1 (50% HP)
            }
        }
    }

    private void CheckForChomp()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return;
        }

        if (canChomp)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            Debug.Log($"Distance to player: {distanceToPlayer}, Chomp Range: {chompRange}");
            if (distanceToPlayer <= chompRange)
            {
                Debug.Log("Chomp action triggered!");
                Chomp(); // Perform the chomp action
            }
        }
        else
        {
            Debug.Log("Chomp is on cooldown.");
        }
    }
    public void Chomp()
    {
        Debug.Log("Chomp action triggered!");

        // Set the sprite to chomp based on health
        spriteRenderer.sprite = health > 25 ? sprites[4] : sprites[5];

        // Start the cooldown for the next chomp
        if (canChomp) // Ensure the coroutine is not started multiple times
        {
            StartCoroutine(ChompCooldown());
        }
    }

    private IEnumerator ChompCooldown()
    {
        Debug.Log("Chomp cooldown started.");
        canChomp = false; // Disable chomping
        yield return new WaitForSeconds(chompCooldown); // Wait for the cooldown duration
        canChomp = true; // Re-enable chomping
        Debug.Log("Chomp cooldown ended.");
    }
    private IEnumerator SwitchIdleSprites()
    {
        while (true)
        {
            if (isMoving)
            {
                // Alternate between idle_1 and idle_2 based on health
                if (health > 25)
                {
                    // Full health: Alternate between sprites[0] (idle_1) and sprites[2] (idle_2)
                    currentIdleIndex = (currentIdleIndex == 0) ? 2 : 0; 
                }
                else
                {
                    // Half health: Alternate between sprites[1] (idle_1, 50% HP) and sprites[3] (idle_2, 50% HP)
                    currentIdleIndex = (currentIdleIndex == 1) ? 3 : 1; 
                }

                // Update the sprite
                spriteRenderer.sprite = sprites[currentIdleIndex];
            }

            yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        }
    }
}