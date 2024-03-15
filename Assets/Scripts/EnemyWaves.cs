using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;


public class EnemyWaves : MonoBehaviour
{
    public EnemyVariables[] enemyVariables; 
    public EnemyVariables activeWave;
    public Vector3[] SpawnPoints;
    float tiempoEspera = 0;
    int waveNum = 0;
    int remainingEnemies = 0;
    public int enemiesAlive;
    public bool hasFinalBoss = false;

    public NavMeshSurface navSurface;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        navSurface.BuildNavMesh();
        NextWave();
        HealthyEntity.DeadEnemy += DeadEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingEnemies > 0 && tiempoEspera <= 0)
        {
            remainingEnemies--;
            tiempoEspera = activeWave.tiempoEntreEnemigos;
            if (activeWave.tipoEnemigo.name != "Enemy Model 5")
            {GameObject newEnemy = Instantiate(activeWave.tipoEnemigo, SpawnPoints[Random.Range(0, SpawnPoints.Length)], Quaternion.identity, parent.transform);}
            else
            {GameObject newEnemy = Instantiate(activeWave.tipoEnemigo, new Vector3(0, 0, 12.5f), Quaternion.identity, parent.transform);}
            
        }
        else
        {
            tiempoEspera -= Time.deltaTime;
        }
    }

    void NextWave()
    {
        if(waveNum < enemyVariables.Length)
        {
            waveNum++;
            activeWave = enemyVariables[waveNum - 1];
            remainingEnemies = activeWave.numeroEnemigos;
            enemiesAlive = activeWave.numeroEnemigos;
            Debug.Log("Oleada: " + waveNum + " Enemigos Vivos: " + enemiesAlive + " Total Rondas: " + enemyVariables.Length);
        }
        else
        { NextLevel();}
    }

    void DeadEnemy()
    {
        if (!hasFinalBoss)
        {
            Debug.Log(" Numero Enemigos: " + activeWave.numeroEnemigos);
            Debug.Log(" Enemigos Vivos: " + enemiesAlive);
            enemiesAlive --;
            Debug.Log(" Enemigos Vivos: " + enemiesAlive);
            if (enemiesAlive <= 0) { NextWave(); }
        }
        else 
        {
            float bossHealth = gameObject.transform.parent.gameObject.transform.parent.Find("Enemy Model 5(Clone)").gameObject.transform.Find("Enemy Body").gameObject.GetComponent<HealthyEntity>().health; 
            if (bossHealth == 0)
            {
                enemiesAlive --;
                if (enemiesAlive <= 0) { NextWave(); }
            }
        }
        
    }

    void NextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level 1") { SceneManager.LoadScene("Level 2"); }
        else if (SceneManager.GetActiveScene().name == "Level 2") { SceneManager.LoadScene("Level 3"); }
        else if (SceneManager.GetActiveScene().name == "Level 3") { SceneManager.LoadScene("Victory Screen"); }
        else { SceneManager.LoadScene("Main Menu"); }
    }
}
