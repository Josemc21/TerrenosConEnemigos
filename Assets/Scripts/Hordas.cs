using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Hordas : MonoBehaviour
{
    public ValoresEnemigos[] valoresEnemigos;
    int numHordaActual = 0;
    bool generandoHorda = false;
    NavMeshSurface navMeshSurface;
    GameObject playerObject;
    public int enemigosRestantes;
    public static int enemigosTotalesRonda;
    public delegate void NextRound();
    public static event NextRound nextRound;

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object not found. Ensure that there is an object tagged as 'Player'.");
        }
        else
        {
            NextHorda();
            EnemyBehaviour.enemyDeath += EnemigoMuerto;
        }
    }

    void NextHorda()
    {
        if (numHordaActual < valoresEnemigos.Length)
        {
            numHordaActual++;
            generandoHorda = true;
            StartCoroutine(GenerarHorda(valoresEnemigos[numHordaActual - 1]));
        }
    }

    IEnumerator GenerarHorda(ValoresEnemigos valores)
    {
        ActualizarEnemigosRestantes();
        enemigosTotalesRonda = enemigosRestantes;
        if (nextRound != null) { nextRound(); }

        foreach (HordaEnemigos horda in valores.hordasEnemigos)
        {
            // Generar la horda actual
            for (int i = 0; i < horda.tiposEnemigos.Length; i++)
            {
                for (int j = 0; j < horda.cantidades[i]; j++)
                {
                    if (!generandoHorda)
                        yield break;

                    Vector3 spawnPosition = RandomNavmeshLocationAroundPlayer();
                    GameObject newEnemy = Instantiate(horda.tiposEnemigos[i], spawnPosition, Quaternion.identity);
                    enemigosRestantes++;

                    yield return new WaitForSeconds(horda.tiempoEntreEnemigos);
                }
            }

            // Esperar hasta que se eliminen todos los enemigos de esta horda
            while (enemigosRestantes > 0)
            {
                yield return null;
            }

            // Retardo entre oleadas basado en valoresEnemigos
            yield return new WaitForSeconds(valores.tiempoEntreHordas);
        }

        // Si se completan todas las hordas, pasar a la siguiente
        NextHorda();
    }

    public void EnemigoMuerto()
    {
        enemigosRestantes--;

        if (enemigosRestantes == 0 && generandoHorda)
        {
            generandoHorda = false;
        }
    }

    void ActualizarEnemigosRestantes()
    {
        enemigosRestantes = 0;
        foreach (HordaEnemigos hordaEnemigos in valoresEnemigos[numHordaActual - 1].hordasEnemigos)
        {
            for (int i = 0; i < hordaEnemigos.tiposEnemigos.Length; i++)
            {
                enemigosRestantes += hordaEnemigos.cantidades[i];
            }
        }
    }

    Vector3 RandomNavmeshLocationAroundPlayer()
    {
        if (playerObject != null)
        {
            // Obtiene la posición del jugador
            Vector3 playerPosition = playerObject.transform.position;

            // Genera una dirección aleatoria en el plano XZ (horizontal)
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0f; // Asegura que la dirección sea horizontal

            // Aplica la distancia fija de 50 unidades alrededor del jugador
            Vector3 randomPosition = playerPosition + randomDirection.normalized * 100f;

            // Muestra la posición aleatoria en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 100f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // Si hay algún problema, devuelve la posición del jugador
        return playerObject.transform.position;
    }

    void OnDestroy()
    {
        EnemyBehaviour.enemyDeath -= EnemigoMuerto;
    }
}
