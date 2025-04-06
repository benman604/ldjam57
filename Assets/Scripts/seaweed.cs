using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seaweed : MonoBehaviour
{
    public Sprite sprite0; // First sprite
    public Sprite sprite1; // Second sprite
    public float animationInterval = 0.5f; // Time between sprite changes

    private SpriteRenderer spriteRenderer;
    private bool useFirstSprite = true; // Toggle between sprites
    private float timer = 0f;

    void Start()
    {
        // Initialize the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite0; // Start with the first sprite
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Switch sprites at regular intervals
        if (timer >= animationInterval)
        {
            timer = 0f; // Reset the timer
            useFirstSprite = !useFirstSprite; // Toggle the sprite
            spriteRenderer.sprite = useFirstSprite ? sprite0 : sprite1;
        }
    }
}