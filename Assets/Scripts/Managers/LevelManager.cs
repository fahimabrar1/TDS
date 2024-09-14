using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Tooltip("The player gameobject")]
    public GameObject plaeyrObj;

    public EnemyWaveGenerator enemyWaveGenerator;
    public LevelMenuUI levelMenuUI;

    public Transform playerSpawnPoint;

    [Tooltip("The player in the level.")]
    public Transform playerTransform;

    [Tooltip("The list of crates in the level.")]
    public List<Transform> crates = new();

    [SerializeField]
    private PlayerController playerController;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of LevelManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            SpawnPlayer();
        }
    }


    public void RemoveCrate(Transform transform)
    {
        crates.Remove(transform);
    }



    public async void OnPlayerDeath()
    {
        enemyWaveGenerator.StopGenerateEnemies();
        levelMenuUI.OnShowButtons();
        await Task.Delay(2000);
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        var obj = Instantiate(plaeyrObj, playerSpawnPoint.position, Quaternion.identity, playerSpawnPoint);
        playerTransform = obj.transform;
        playerController = playerTransform.GetComponent<PlayerController>();
    }




    public void OnStart()
    {
        enemyWaveGenerator.StartGenerateEnemies();
    }



    public PlayerController GetPlayer()
    {
        return playerController;
    }
}
