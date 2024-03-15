using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    public GameObject[] enemyModels;
    public Vector3[] SpawnPoints;
    float tiempoEspera = 6;
    bool canSpawn = false;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && canSpawn)
        {
            tiempoEspera = 6;
            GameObject newEnemy = Instantiate(enemyModels[Random.Range(0, enemyModels.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)], Quaternion.identity);
            canSpawn = false;
        }
        else 
        {
            tiempoEspera -= Time.deltaTime;
            if (tiempoEspera < 0) { canSpawn = true; }
        }
    }
}
