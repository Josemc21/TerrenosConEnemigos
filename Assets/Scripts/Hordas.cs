using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class Hordas : MonoBehaviour
{
    public ValoresEnemigos[] valoresEnemigos;
    int numHordaActual = 0;
    bool generandoHorda = false;
    //public GameObject textoNumHordas;
    NavMeshSurface navMeshSurface;
    Transform playerTransform;
    public int enemigosRestantes;
    public static int enemigosTotalesRonda;
    public delegate void NextRound();
    public static event NextRound nextRound;

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        playerTransform = transform;
        NextHorda();
        EnemyBehaviour.enemyDeath += EnemigoMuerto;
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

                    Vector3 spawnPosition = RandomNavmeshLocationAroundPlayer(200f, 200f);
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

    Vector3 RandomNavmeshLocationAroundPlayer(float minRadius, float maxRadius)
    {
        // Obtiene el centro del NavMesh
        Vector3 navMeshCenter = navMeshSurface.transform.position;

        // Genera una dirección aleatoria en el plano XZ (horizontal)
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f; // Asegura que la dirección sea horizontal

        // Genera una distancia aleatoria dentro del rango especificado
        float distance = Random.Range(minRadius, maxRadius);

        // Aplica la distancia aleatoria a la dirección aleatoria
        Vector3 randomPosition = navMeshCenter + distance * randomDirection.normalized;

        // Muestra la posición aleatoria en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, maxRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            // Si la muestra de posición falla, devuelve la posición del jugador
            return playerTransform.position;
        }
    }

    void OnDestroy()
    {
        EnemyBehaviour.enemyDeath -= EnemigoMuerto;
    }
}
