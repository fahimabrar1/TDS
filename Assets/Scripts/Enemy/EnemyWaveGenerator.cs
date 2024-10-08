using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading.Tasks;

public class EnemyWaveGenerator : MonoBehaviour
{
    [Tooltip("Prefab of the enemy to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Time delay between each wave.")]
    public float waveDelay = 4f;
    public Transform wavePosition;

    public List<Enemy> enemyList = new(); // Queue to store enemies in a wave

    private Enemy currentEnemy;

    private bool canGenerateEnemies = false;


    // Method to spawn enemies in waves
    public void SpawnWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn enemy at a random spawn point
            GameObject enemyObj = Instantiate(enemyPrefab, wavePosition.position, Quaternion.identity);

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.SetWaveGenerator(this);


            currentEnemy = enemy;

            // Add to the queue
            enemyList.Add(enemy);

            // Assign front and behind relationships
            if (enemyList.Count > 1)
            {
                Enemy enemyInFront = enemyList[enemyList.Count - 2];
            }
        }
    }



    // Coroutine to spawn waves with delays
    private IEnumerator SpawnWavesRoutine()
    {
        while (canGenerateEnemies)
        {
            SpawnWave(1);  // Spawn 5 enemies per wave (adjust as needed)
            yield return new WaitForSeconds(waveDelay);
        }
    }

    public async void StartGenerateEnemies()
    {
        canGenerateEnemies = true;
        await Task.Delay(1000);
        StartCoroutine(SpawnWavesRoutine());
    }


    public void StopGenerateEnemies()
    {
        canGenerateEnemies = false;
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            try
            {
                Destroy(enemyList[i].gameObject);
            }
            catch (System.Exception)
            {
            }
        }
    }


    public void EnemyDeathNotify(Enemy enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
