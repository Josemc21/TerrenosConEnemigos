using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    NavMeshAgent pathfinder;
    Transform target;
    float remainingHealth;
    public GameObject healthBar;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pathfinder = GetComponent<NavMeshAgent>();
        remainingHealth = GetComponent<HealthManager>().maxHealth;
    }

    void FixedUpdate()
    {
        pathfinder.SetDestination(target.position);
    }

    public void Hit(float damage)
    {
        // Reduce health
        remainingHealth -= damage;

        // Update health bar
        UpdateHealthBar();

        if (remainingHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        float healthRatio = remainingHealth / GetComponent<HealthManager>().maxHealth;
        healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);
    }
}
