using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyWaveGenerator : MonoBehaviour
{
    [Tooltip("Prefab of the enemy to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Spawn points for the enemies.")]
    public List<Transform> spawnPoints;

    [Tooltip("Time delay between each wave.")]
    public float waveDelay = 4f;
    public Transform wavePosition;

    private Queue<Zombie> enemyQueue = new(); // Queue to store enemies in a wave

    private Zombie currentEnemy;

    // Start spawning waves
    private void Start()
    {
        StartCoroutine(SpawnWavesRoutine());
    }


    // Method to spawn enemies in waves
    public void SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn enemy at a random spawn point
            GameObject enemyObj = Instantiate(enemyPrefab, wavePosition.position, Quaternion.identity);

            Zombie enemy = enemyObj.GetComponent<Zombie>();

            if (currentEnemy != null)
            {
                currentEnemy.SetEnemyBehind(enemy);
                enemy.SetEnemyInFront(currentEnemy);
            }
            currentEnemy = enemy;

            // Add to the queue
            enemyQueue.Enqueue(enemy);

            // Assign front and behind relationships
            if (enemyQueue.Count > 1)
            {
                Zombie enemyInFront = enemyQueue.ToArray()[enemyQueue.Count - 2];
                enemy.SetEnemyInFront(enemyInFront);
                enemyInFront.SetEnemyBehind(enemy);
            }
        }
    }



    // Coroutine to spawn waves with delays
    private IEnumerator SpawnWavesRoutine()
    {
        while (true)
        {
            SpawnWave(1);  // Spawn 5 enemies per wave (adjust as needed)
            yield return new WaitForSeconds(waveDelay);
        }
    }
}
