using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBehaviour : HealthyEntity
{
    // Enemy Variables
    float enemyCollisionRadius;                         // Enemy Model Collision Radius
    float attackDistance = 2.5f;                        
    bool onAttack = false;                              
    float attackRate = 0.5f;
    float attackCheck = 0;
    float damage = 1;      
    bool spawning = true;                               // Spawn Check           
    public Material skin;
    public Material transparentSkin;                  
    UnityEngine.AI.NavMeshAgent pathFinder;             // AI PathFinder to Target

    // Target Variables
    Transform target;                                   // Target Object
    float targetCollisionRadius;                        // Target Model Collision Radius
    HealthyEntity targetEntity;                         // Target Health Control
    bool madeContact = false;                           // Target Contact Control

    // Game Variables
    bool endGame = false;


    protected override void Start()
    {
        base.Start();  

        // Spawn Sequence
        Debug.Log("Cambio skin a transparente");
        gameObject.GetComponent<Renderer>().material = transparentSkin;
        gameObject.transform.parent.gameObject.transform.Find("Spawn Indicator").gameObject.SetActive(true);
        StartCoroutine(Spawn());

        

        // Enemy Variables init                                                                 
        pathFinder = GetComponent<UnityEngine.AI.NavMeshAgent>();     
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;

        // Target Variables init
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetEntity = target.GetComponent<HealthyEntity>();
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        // Player Death Listener
        PlayerControl.PlayerDeath += EndGame;
    }

    void Update()
    {
        if (!endGame && !spawning && !onAttack)
        {
            UnityEngine.Vector3 dirToTarget = (target.position - transform.position).normalized;
            UnityEngine.Vector3 targetPosition = target.position - dirToTarget * (enemyCollisionRadius + targetCollisionRadius + attackDistance);
            pathFinder.SetDestination(targetPosition);

            if (Time.time > attackCheck)
            {
                attackCheck = Time.time + attackRate;
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget <= Mathf.Pow(enemyCollisionRadius + targetCollisionRadius + attackDistance, 2))
                {
                    Debug.Log("Enemy in range");
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        if (!endGame && !spawning) 
        {
            pathFinder.enabled = false;         
            onAttack = true;
            gameObject.transform.Find("Attack Indicator").gameObject.SetActive(true);

            Debug.Log("Enemy initiating attack secuence!");
            yield return new WaitForSeconds(0.5f);

            UnityEngine.Vector3 targetPosition = transform.position;
            UnityEngine.Vector3 dirToTarget = (target.position - transform.position).normalized;
            UnityEngine.Vector3 attackPosition = target.position - dirToTarget * (enemyCollisionRadius + targetCollisionRadius);

            float percent = 0;
            float attackSpeed = 1;          // Enemy MovementSpeed Attacking
            bool hasAppliedDmg = false;
            madeContact = false;

            while (percent <= 1)
            {
                if (percent >= 0.5f && !hasAppliedDmg && madeContact)
                {
                    targetEntity.TakeDamage(damage);
                    hasAppliedDmg = true;
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolacion = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = UnityEngine.Vector3.Lerp(targetPosition, attackPosition, interpolacion);
                yield return null;
            }
            
            pathFinder.enabled = true;
            onAttack = false;
            gameObject.transform.Find("Attack Indicator").gameObject.SetActive(false);
        }
    }

    // Stop Enemy Movement on Game End
    void EndGame() 
    { 
        endGame = true; 
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") { madeContact = true; }
    }

    IEnumerator Spawn()
    {
        Debug.Log("Enemy spawning");
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Renderer>().material = skin;
        gameObject.transform.parent.gameObject.transform.Find("Spawn Indicator").gameObject.SetActive(false);
        if (skin.name == "Demon Boss Skin")
        {
            gameObject.transform.Find("Horns").gameObject.SetActive(true);
            gameObject.transform.Find("Trident").gameObject.SetActive(true);
        }
        Debug.Log("Cambio skin a original");
        spawning = false;
    }
}
