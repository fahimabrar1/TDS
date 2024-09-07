using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    public int health = 100;
    public GameObject cratePrefab;        // Crate prefab to spawn
    public Transform feetPosition;        // Position at player's feet for crate spawning

    private readonly List<GameObject> spawnedCrates = new();



    private void Update()
    {

        // Crate spawning input
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnCrate();
        }
    }



    private void SpawnCrate()
    {
        Vector3 spawnPosition = feetPosition.position;

        // If there's already a crate, stack the new one on top
        if (spawnedCrates.Count > 0)
        {
            GameObject topCrate = spawnedCrates.Last();
            spawnPosition = new Vector3(topCrate.transform.position.x, topCrate.transform.position.y + topCrate.transform.localScale.y, topCrate.transform.position.z);
        }

        GameObject newCrate = Instantiate(cratePrefab, spawnPosition, Quaternion.identity);
        spawnedCrates.Add(newCrate);
    }

    public void OnTakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage. Health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Handle player death (respawn or game over logic)
    }
}
