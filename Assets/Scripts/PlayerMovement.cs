using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float spinSpeed = 10f;

    Rigidbody2D rb;
    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();   
        cameraTransform = Camera.main.transform;
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
            return;
        }

        // rb.angularVelocity = -moveX * spinSpeed;

        // Vector2 forward = transform.up;
        rb.velocity = new Vector2(moveX, moveY) * moveSpeed;
    }
}
