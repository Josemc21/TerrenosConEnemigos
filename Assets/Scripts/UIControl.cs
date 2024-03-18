using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("Timer")]
    public Text timer;
    private float elapsedTime;

    [Header("Round Counter")]
    public Text roundNumber;
    private int activeRound = 1;

    [Header("Enemy Counter")]
    private int totalEnemies;
    public Text totalCounter;
    private bool reduceEnemy;
    private float enemiesAlive = 0;

    private bool roundStart = true;

    void Start()
    {   
        // Enemy Death Subscription
        EnemyBehaviour.enemyDeath += UpdateAliveCounter;
        Hordas.nextRound += UpdateRoundCounter;
        Hordas.nextRound += UpdateEnemyCounter;
    }

    void Update()
    {

        /*************  ENEMY COUNTER  *************/

        // Enemy Death Regulator (function is called twice for unknown reason)
        reduceEnemy = true;

        // Get Total Enemies on Round Start
        if (roundStart)
        {
            totalEnemies = Hordas.enemigosTotalesRonda;
            roundStart = false;
        }

        // Update Counter
        totalCounter.text = "" + enemiesAlive + " / " + totalEnemies;

        /*************  TIMER  *************/
        
        // Calculate time
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }

    void UpdateAliveCounter() 
    { 
        if (reduceEnemy) 
        { 
            enemiesAlive++; 
            reduceEnemy = false;
        }
    }

    /*************  ROUND COUNTER  *************/
    void UpdateRoundCounter()
    {
        activeRound++;
        roundNumber.text = "" + activeRound;
    }

    void UpdateEnemyCounter() {
        totalEnemies = Hordas.enemigosTotalesRonda;
        enemiesAlive = 0;
    }
}