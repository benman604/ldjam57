using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float moveSpeedWhenChargingMultiplier = 0.15f;
    public float spinSpeed = 10f;

    public float chargeTime = 3f; // Time required to fully charge the attack
    public Sprite[] chargeSprites; // Assign the 7 sprites in the Inspector (index 0 to 6)

    public Gun[] guns;

    private Rigidbody2D rb;
    private Transform cameraTransform;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private bool isCharging = false;
    private bool isFullyCharged = false; // Track if the player is fully charged

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();   
        cameraTransform = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            cameraTransform.position.z
        ); 

        // Prevent movement while charging
        // if (!isCharging)
        // {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (moveX == 0 && moveY == 0)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = new Vector2(moveX, moveY) * moveSpeed * (isCharging ? moveSpeedWhenChargingMultiplier : 1f); 
            }
        // }
        // else
        // {
            // rb.velocity = Vector2.zero; // Stop movement while charging
        // }

        // Handle charging and shooting
        if (Input.GetKeyDown(KeyCode.Space) && !isCharging)
        {
            StartCoroutine(ChargeAndShoot());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            if (isFullyCharged) {
                foreach (var gun in guns) {
                    if (gun != null) {
                        gun.Fire();
                    }
                }
                isFullyCharged = false;
                spriteRenderer.sprite = chargeSprites[0];
            } else {
                spriteRenderer.sprite = chargeSprites[0];
            }
        }
    }

    private IEnumerator ChargeAndShoot()
    {
        isCharging = true;
        float elapsedTime = 0f;
        int spriteIndex = 0;

        // Charging phase
        while (Input.GetKey(KeyCode.Space) && elapsedTime < chargeTime)
        {
            elapsedTime += Time.deltaTime;

            // Change sprite every fractional second relative to chargeTime/3
            if (elapsedTime >= (spriteIndex + 1) * (chargeTime / 3f) && spriteIndex < 3) {
                spriteIndex++;
                spriteRenderer.sprite = chargeSprites[spriteIndex * 2]; // Use sprites 1, 3, 5, 7
            }

            yield return null;
        }

        // Shooting phase
        if (elapsedTime >= chargeTime)
        {
            spriteRenderer.sprite = chargeSprites[6]; // Fully charged sprite (sprite7)
            // Shoot();
            isFullyCharged = true; // Mark as fully charged
            // yield return new WaitForSeconds(0.5f); // Keep the fully charged sprite for 0.5 seconds
        }

        // Reset sprite to normal after shooting
        // spriteRenderer.sprite = chargeSprites[0];

        // isCharging = false;
    }

    // private void Shoot()
    // {
    //     if (projectilePrefab != null && firePoint != null)
    //     {
    //         GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    //         Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
    //         if (projectileRb != null)
    //         {
    //             projectileRb.velocity = firePoint.right * fireSpeed;
    //         }
    //         else {
    //             Debug.LogWarning("Projectile Rigidbody2D component not found.");
    //         }
    //     }
    // }
}