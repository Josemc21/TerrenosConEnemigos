using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class HealthyEntity : MonoBehaviour, IHealthControl
{
    protected bool dead;
    public float health;
    public float startingHealth;
    public delegate void OnDeadEnemy();
    public static event OnDeadEnemy DeadEnemy;

    AudioManagement audioManager;

    protected virtual void Start()
    {
        health = startingHealth;
        dead = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagement>();
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        TakeDamage(damage);
        // Raycast problems, sustitute function applied
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Hit! " + health + " hits left.");
        if (gameObject.tag == "Player") { audioManager.PlaySFX(audioManager.hurtPlayer); } else { audioManager.PlaySFX(audioManager.hurtEnemy); }
        if (health <= 0 && !dead) { Die(); }
    }

    public void Die()
    {
        if (gameObject.tag == "Player")
        {
            Debug.Log("Player down!");
            dead = true;
            audioManager.PlaySFX(audioManager.deathPlayer); 
            Destroy(gameObject);
            if (DeadEnemy != null) { DeadEnemy(); }
        }
        else 
        {
            Debug.Log("Enemy down!");
            dead = true;
            audioManager.PlaySFX(audioManager.deathEnemy);
            Destroy(gameObject.transform.parent.gameObject);
            if (DeadEnemy != null) { DeadEnemy(); }
        }
        
    }
}
