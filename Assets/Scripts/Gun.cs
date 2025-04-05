using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float fireRate = 0.5f; 
    public float bulletSpeed = 10f; 

    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Fire()
    {
        animator.Play("gun_fire");
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = firePoint.right * bulletSpeed;
            }
            else {
                Debug.LogWarning("Projectile Rigidbody2D component not found.");
            }
        }
    }
}
