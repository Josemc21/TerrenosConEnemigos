using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float health = 5f;
    public float maxHealth = 5f;

    public UnityEvent beenHit;
    public UnityEvent dead;


    void Hit(float damage)
    {
        Debug.Log("I got hit");
        health -= damage;
        beenHit.Invoke();
        if (health <= 0)
        {
            dead.Invoke();
        }
    }
}
