using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{


    public float health = 100f;
    public float moveSpeed = 5f; 
    public float moveSpeedWhenChargingMultiplier = 0.15f;
    public float spinSpeed = 10f;

    
    public Image[] heartImages; // Assign the 4 heart images in the Inspector
    public Sprite fullHeartSprite; // Assign the full heart sprite in the Inspector
    public Sprite emptyHeartSprite; // Assign the empty heart sprite in the Inspector

    public Gun gunPrefab;
    public Transform[] gunPositions;
    int gunIndex = 0; 


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

    Blinker blinker;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();   
        cameraTransform = Camera.main.transform;
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        blinker = gameObject.AddComponent<Blinker>();
        defaultAngularDrag = rb.angularDrag; 
    }



    private void UpdateHealthBar()
    {
        int fullHearts = Mathf.FloorToInt(health / 25f); // Calculate the number of full hearts

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < fullHearts)
            {
                heartImages[i].sprite = fullHeartSprite; // Set to full heart
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite; // Set to empty heart
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("collectable_gun"))
        {
            Destroy(collision.gameObject);
            if (gunIndex < guns.Length) { // Ensure gunIndex is within bounds
                Gun newGun = Instantiate(gunPrefab, gunPositions[gunIndex].position, gunPositions[gunIndex].rotation);
                newGun.transform.SetParent(transform); 
                guns[gunIndex] = newGun; 
                gunIndex++;
            } else {
                Debug.Log("No more gun positions available.");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(blinker.blinkFor(0.5f));
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player has died.");
            SceneManager.LoadScene("LossScene");
        }

        UpdateHealthBar(); // Update the health bar

        if (healthText != null)
        {
            healthText.SetText(health.ToString("F0")); // Update the health text display
        }
        else
        {
            Debug.LogWarning("healthText is not assigned in the Inspector.");
        }
    }

    public void Die() {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }
}