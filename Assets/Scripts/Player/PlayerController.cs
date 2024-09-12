using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerDamagable
{
    [Tooltip("The player's initial health.")]
    private int health = 100;
    public HealthBar healthBar;

    [Tooltip("The prefab for the crate that the player can spawn.")]
    public ICrateSpawner crateSpawner;

    [Header("Throwable Data")]

    [Tooltip("The prefab for the grenade that the player can throw.")]
    public GameObject throwablePrefab;

    [Tooltip("The force applied when the grenade is thrown.")]
    public float throwableThrowForce = 4f;
    [Tooltip("The force applied when the grenade is thrown to spin.")]
    public float throwableSpinForce = 0.2f;

    [Tooltip("The point from where the throwables is thrown")]
    public Transform throwingPoint;



    [Header("Weapon Data")]
    [Tooltip("The current weapon equipped by the player.")]
    public Weapon currentWeapon;

    [Tooltip("The main camera in the scene, used for touch input.")]
    public Camera mainCamera;




    [Tooltip("A list of all enemies in the scene.")]
    public List<Enemy> enemies = new List<Enemy>();


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (TryGetComponent(out CrateSpawner crateSpawner))
        {
            this.crateSpawner = crateSpawner;
        }

        mainCamera = Camera.main;
    }


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        healthBar.InitializeHealthBar(health);
    }


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


        // Handle grenade throwing logic
        if (Input.GetKeyUp(KeyCode.G))  // Press 'G' to throw a grenade
        {
            ThrowGrenade();
        }


        // Handle grenade throwing logic
        if (Input.GetKeyUp(KeyCode.P))  // Press 'G' to throw a grenade
        {
            currentWeapon.ActivatePoweredMode();
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
    /// Throws a grenade from the player's throwing point at a 45-degree angle with a spin.
    /// </summary>
    private void ThrowGrenade()
    {
        if (throwablePrefab == null || throwingPoint == null) return;
        MyDebug.Log($"Throwing Grenade.");

        // Instantiate the grenade at the throwing point
        GameObject grenade = Instantiate(throwablePrefab, throwingPoint.position, Quaternion.identity);

        // Get the Rigidbody2D of the grenade to apply force and torque (spin)
        if (grenade.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            // Calculate the 45-degree direction
            float angleInRadians = 45 * Mathf.Deg2Rad;
            Vector2 throwDirection = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            // Apply a forward force to throw the grenade
            rb2d.AddForce(throwDirection * throwableThrowForce, ForceMode2D.Impulse);

            // Apply a random spin to the grenade (adjust the value for more or less spin)
            float spinForce = Random.Range(-throwableSpinForce, throwableSpinForce);
            rb2d.AddTorque(spinForce, ForceMode2D.Impulse);
        }
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
    [Button("Spawn Crate")]
    public void SpawnCrate()
    {
        crateSpawner.SpawnCrate(transform);
    }


    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    public void OnTakeDamage(int damage)
    {
        healthBar.DeduceHealth(damage);

        healthBar.ShowHealthbar();
        MyDebug.Log($"Player took {damage} damage. Health: {healthBar.currentHealth}");

        if (healthBar.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            healthBar.UpdateHealthbar();
        }
    }


    /// <summary>
    /// Called when the player dies.
    /// </summary>
    private void Die()
    {
        // Handle  death (e.g., despawn, play death animation)
        healthBar.KillHealthTween();
        MyDebug.Log("Player died!");
        // Handle player death (respawn or game over logic)
    }


    public Transform GetTransform()
    {
        return transform;
    }
}
