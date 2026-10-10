using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Configuración de Oleadas")]
    public GameObject[] zombiePrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public float initialDelay = 2f;

    private float timer;

    void Start()
    {
        timer = spawnInterval - initialDelay;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomZombie();
            timer = 0f;
        }
    }

    void SpawnRandomZombie()
    {
        if (zombiePrefabs.Length == 0 || spawnPoints.Length == 0) return;

        int randomZombieIndex = Random.Range(0, zombiePrefabs.Length);
        int randomPointIndex = Random.Range(0, spawnPoints.Length);

        Transform selectedSpawn = spawnPoints[randomPointIndex];

        if (selectedSpawn != null)
        {
            GameObject newZombie = Instantiate(zombiePrefabs[randomZombieIndex], selectedSpawn.position, Quaternion.identity);
            newZombie.transform.position = selectedSpawn.position;
        }
    }

    // NUEVA FUNCIÓN: Se llama desde el GameManager cuando sale el Jefe
    public void ActivarHordaFinal(float nuevoIntervalo)
    {
        spawnInterval = nuevoIntervalo; 
        timer = spawnInterval; // Fuerza a que salga un zombi inmediatamente
        Debug.Log("¡Horda final activada! Los zombis salen cada " + spawnInterval + " segundos.");
    }
}