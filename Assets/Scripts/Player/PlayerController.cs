using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Tooltip("The player's initial health.")]
    public int health = 100;

    [Tooltip("The prefab for the crate that the player can spawn.")]
    public GameObject cratePrefab;

    [Tooltip("The current weapon equipped by the player.")]
    public Weapon currentWeapon;

    [Tooltip("The main camera in the scene, used for touch input.")]
    public Camera mainCamera;

    [Tooltip("A list of all enemies in the scene.")]
    public List<Enemy> enemies = new List<Enemy>();

    [Tooltip("A list of all crates spawned by the player.")]
    private readonly List<GameObject> spawnedCrates = new();


    /// <summary>
    /// Updates the player's state every frame.
    /// </summary
    private void Update()
    {
        // Handle player shooting logic
        if (Input.GetMouseButton(0))
        {
            HandleTouchInput();  // Aim and shoot towards touch input
        }
        else
        {
            ShootClosestEnemy();  // Automatically shoot the closest enemy
        }
    }


    /// <summary>
    /// Handles touch input from the player.
    /// </summary>
    private void HandleTouchInput()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0; // Keep the z-position at 0 for 2D

        // Aim and shoot in the direction of touch
        AimAndShoot(touchPosition);
        currentWeapon.ShowShootingRadius(true);
    }


    /// <summary>
    /// Shoots the closest enemy to the player.
    /// </summary>
    private void ShootClosestEnemy()
    {
        currentWeapon.ShowShootingRadius(false);
        Vector3 closestEnemyPos = GetClosestEnemyPosition();
        if (closestEnemyPos != Vector3.zero)
        {
            AimAndShoot(closestEnemyPos);
        }
    }


    /// <summary>
    /// Aims and shoots at a given position.
    /// </summary>
    /// <param name="point">The position to aim and shoot at.</param>
    private void AimAndShoot(Vector3 point)
    {
        currentWeapon.AimAt(point);
        currentWeapon.Attack(point);
    }


    /// <summary>
    /// Gets the position of the closest enemy to the player.
    /// </summary>
    /// <returns>The position of the closest enemy.</returns>
    private Vector3 GetClosestEnemyPosition()
    {
        Vector3 closestEnemy = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy.transform.position;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }


    /// <summary>
    /// Spawns a crate at the player's feet or on top of existing crates.
    /// </summary>
    public void SpawnCrate()
    {
        Vector3 spawnPosition = transform.position;

        // If there's already a crate, stack the new one on top
        if (spawnedCrates.Count > 0)
        {
            GameObject topCrate = spawnedCrates[^1];  // ^1 is the same as .Last()
            spawnPosition = new Vector3(topCrate.transform.position.x, topCrate.transform.position.y + topCrate.transform.localScale.y, topCrate.transform.position.z);
        }

        GameObject newCrate = Instantiate(cratePrefab, spawnPosition, Quaternion.identity);
        spawnedCrates.Add(newCrate);
    }


    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    public void OnTakeDamage(int damage)
    {
        health -= damage;
        MyDebug.Log($"Player took {damage} damage. Health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }


    /// <summary>
    /// Called when the player dies.
    /// </summary>
    private void Die()
    {
        MyDebug.Log("Player died!");
        // Handle player death (respawn or game over logic)
    }
}
