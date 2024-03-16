using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Experimental;

public class UIControl : MonoBehaviour
{
    [Header("Timer")]
    public Text timer;
    private float elapsedTime;

    [Header("Round Counter")]
    public Text roundNumber;
    private int activeRound = 1;
    private bool roundIncreased = false;

    [Header("Enemy Counter")]
    private GameObject[] totalEnemies;
    public Text totalCounter;
    private bool reduceEnemy;
    private float enemiesAlive = 0;

    private bool roundStart = true;

    void Start()
    {   
        // Enemy Death Subscription
        EnemyBehaviour.enemyDeath += UpdateAliveCounter;
    }

    void Update()
    {

        /*************  ENEMY COUNTER  *************/

        // Enemy Death Regulator (function is called twice for unknown reason)
        reduceEnemy = true;

        // Get Total Enemies on Round Start
        if (roundStart)
        {
            totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            enemiesAlive = totalEnemies.Length;
            roundStart = false;
        }

        // Update Counter
        totalCounter.text = "" + enemiesAlive + " / " + totalEnemies.Length;

        /*************  TIMER  *************/
        
        // Calculate time
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); 

        /*************  ROUND COUNTER  *************/

        // Round Update
        if (enemiesAlive <= 0 && !roundStart && !roundIncreased) {
            activeRound++;
            roundIncreased = true;
        }
        roundNumber.text = "" + activeRound;
    }

    void UpdateAliveCounter() 
    { 
        if (reduceEnemy) 
        { 
            enemiesAlive--; 
            reduceEnemy = false;
        }
    }
}
