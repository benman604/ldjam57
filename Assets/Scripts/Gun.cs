using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    public float fireRate = 0.5f; 
    public float bulletSpeed = 10f; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D projectileRb = projectilePrefab.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = firePoint.right * bulletSpeed;
            }
        }
    }
}
