using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWaveGenerator : MonoBehaviour
{
    [Tooltip("Prefab of the enemy to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Initial delay between waves.")]
    public float initialWaveDelay = 4f;

    [Tooltip("How fast the delay decreases between waves.")]
    public float waveAcceleration = 0.1f;  // Lower means faster acceleration

    [Tooltip("Minimum delay between waves.")]
    public float minimumWaveDelay = 1f;  // Minimum time between waves

    [Tooltip("Number of waves before taking a break.")]
    public int wavesBeforeBreak = 5;

    [Tooltip("Duration of the break between wave sets.")]
    public float breakDuration = 6f;

    [Tooltip("The number of enemies to start with in each wave.")]
    public int initialEnemiesPerWave = 1;

    [Tooltip("Position where enemies will spawn.")]
    public Transform wavePosition;

    [Tooltip("Minimum random delay between enemy spawns (in seconds).")]
    public float minSpawnDelay = 0.1f;

    [Tooltip("Maximum random delay between enemy spawns (in seconds).")]
    public float maxSpawnDelay = 1f;

    private Queue<Zombie> enemyQueue = new();
    private Zombie lastSpawnedEnemy;
    private float currentWaveDelay;
    private int currentWaveCount = 0;
    private int currentEnemiesPerWave;

    // Start spawning waves
    private void Start()
    {
        currentWaveDelay = initialWaveDelay;
        currentEnemiesPerWave = initialEnemiesPerWave;
        StartCoroutine(SpawnWavesRoutine());
    }

    // Method to spawn enemies in a single wave
    public IEnumerator SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn enemy at the wave position
            GameObject enemyObj = Instantiate(enemyPrefab, wavePosition.position, Quaternion.identity);
            Zombie newEnemy = enemyObj.GetComponent<Zombie>();

            // Update the last spawned enemy relationship logic if needed
            // if (lastSpawnedEnemy != null)
            // {
            //     lastSpawnedEnemy.SetEnemyBehind(newEnemy);
            //     newEnemy.SetEnemyInFront(lastSpawnedEnemy);
            // }

            // Update the last spawned enemy
            lastSpawnedEnemy = newEnemy;

            // Add the new enemy to the queue
            enemyQueue.Enqueue(newEnemy);

            // Random delay before the next enemy spawn
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);  // Wait for a random time
        }
    }

    // Coroutine to spawn waves with delays and breaks
    private IEnumerator SpawnWavesRoutine()
    {
        while (true)
        {
            // Spawn the current wave of enemies with random delays between each enemy
            yield return StartCoroutine(SpawnWave(currentEnemiesPerWave));

            // Increment the wave counter
            currentWaveCount++;

            // Check if a break should happen after a set number of waves
            if (currentWaveCount >= wavesBeforeBreak)
            {
                // Take a break after a number of waves
                yield return new WaitForSeconds(breakDuration);
                currentWaveCount = 0; // Reset wave count after the break
            }
            else
            {
                // Wait for the current delay before spawning the next wave
                yield return new WaitForSeconds(currentWaveDelay);

                // Decrease the delay between waves to increase spawn rate, but not below the minimum
                currentWaveDelay = Mathf.Max(currentWaveDelay - waveAcceleration, minimumWaveDelay);
            }

            // Optionally increase the number of enemies per wave over time
            currentEnemiesPerWave++;
        }
    }
}
