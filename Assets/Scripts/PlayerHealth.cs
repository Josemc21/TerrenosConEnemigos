using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float startingHealth = 10;
    public float remainingHealth;
    public GameObject healthBar;
    public Rigidbody rb;
    private bool gotHit = false;
    private float dmgCooldown = 1.0f;
    private float dmgCooldownCounter = 0;


    void Start()
    {
        remainingHealth = startingHealth;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gotHit) { dmgCooldownCounter += Time.deltaTime;}
        if (dmgCooldownCounter >= dmgCooldown) { gotHit = false; dmgCooldownCounter = 0; }
        if (remainingHealth <= 0) { SceneManager.LoadScene("Death Screen"); }
    }

    void UpdateHealthBar()
    {
        float healthRatio = remainingHealth / startingHealth;
        healthBar.transform.localScale = new Vector3(healthRatio, 1, 1);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "EnemyBody" && !gotHit)
        {
            remainingHealth--;
            UpdateHealthBar();
            gotHit = true;
        }
    }
}
