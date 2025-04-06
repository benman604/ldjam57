using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Health of the enemy
    public float distToSmell = 10f; 
    public PlayerMovement player;
    public bool isDead = false;
    NavMeshAgent agent;

    // Start is called before the first frame update
    protected void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!isDead && Vector3.Distance(transform.position, player.gameObject.transform.position) <= distToSmell)
        {
            agent.SetDestination(player.gameObject.transform.position);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        Animator animator = GetComponent<Animator>();
        animator.Play("shark_dead");
        isDead = true;
        Destroy(gameObject, 2f); 
    }
}