using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
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
    }

    // Update is called once per frame
    void Update()
    {
        pathfinder.SetDestination(target.position);
    }

    public void beenHit()
    {
        Debug.Log("" + gameObject.name + " has been hit!");
        remainingHealth = GetComponent<HealthManager>().health / GetComponent<HealthManager>().maxHealth;
        healthBar.transform.localScale = new Vector3(remainingHealth, 1, 1);
    }

    public void gotKilled()
    {
        Debug.Log("" + gameObject.name + " has been killed!");
    }
}
