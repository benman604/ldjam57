using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 5f; 
    public float moveSpeedWhenChargingMultiplier = 0.15f;
    public float spinSpeed = 10f;

    public float drag = 3f;

    public float chargeTime = 3f; // Time required to fully charge the attack
    public Sprite[] chargeSprites; // Assign the 7 sprites in the Inspector (index 0 to 6)

    public Gun[] guns;
    public float gunsDamage = 50f;

    public TextMeshProUGUI healthText;

    private Rigidbody2D rb;
    private Transform cameraTransform;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private bool isCharging = false;
    private bool isFullyCharged = false; // Track if the player is fully charged
    private int spinDir = 1;
    private float defaultAngularDrag; 

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();   
        cameraTransform = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        defaultAngularDrag = rb.angularDrag; 
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            cameraTransform.position.z
        ); 


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

        if (rb.velocity.magnitude > 0) {
            rb.angularVelocity = -spinDir * spinSpeed * (isCharging ? moveSpeedWhenChargingMultiplier : 1f);
        }

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
                        gun.Fire(gunsDamage); 
                    }
                }
                isFullyCharged = false;
                spriteRenderer.sprite = chargeSprites[0];
            } else {
                spriteRenderer.sprite = chargeSprites[0];
            }
        }

        // Spin left on Q and right on E
        if (Input.GetKeyDown(KeyCode.Q)) {
            spinDir = -1;
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            spinDir = 1;
        }

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) {
            rb.angularVelocity += -spinDir * spinSpeed * Time.deltaTime; 
        }

        // Increase angular drag on Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            rb.angularDrag = drag;
        } else {
            rb.angularDrag = defaultAngularDrag; 
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

            if (elapsedTime >= (spriteIndex + 1) * (chargeTime / 3f) && spriteIndex < 3) {
                spriteIndex++;
                spriteRenderer.sprite = chargeSprites[spriteIndex * 2];
            }

            yield return null;
        }

        // Shooting phase
        if (elapsedTime >= chargeTime)
        {
            spriteRenderer.sprite = chargeSprites[6]; 
            isFullyCharged = true; 
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // Handle player death here (e.g., restart the level, show game over screen, etc.)
            Debug.Log("Player has died.");
        }

        healthText.SetText(health.ToString("F0")); // Update the health text display
    }

    public void Die() {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }
}