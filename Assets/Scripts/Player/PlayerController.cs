using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IPlayerDamagable
{
    [Tooltip("The player's initial health.")]
    private int health = 100;
    public HealthBar healthBar;
    public Transform feetTransform;

    [Tooltip("The prefab for the crate that the player can spawn.")]
    public ICrateSpawner crateSpawner;

    [Header("Throwable Data")]
    public ThrowableGranadeDataSO ThrowableGranadeData;

    [Tooltip("The point from where the throwables is thrown")]
    public Transform throwingPoint;


    [Header("Weapon Data")]
    [Tooltip("The current weapon equipped by the player.")]
    public Weapon currentWeapon;

    [Tooltip("The main camera in the scene, used for touch input.")]
    public Camera mainCamera;

    public bool isPlayerAlive;


    [Tooltip("A list of all enemies in the scene.")]
    public List<Enemy> enemies = new();
    private EnemyWaveGenerator enemyWaveGenerator;


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
        enemyWaveGenerator = FindAnyObjectByType<EnemyWaveGenerator>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        isPlayerAlive = true;
        StartCoroutine(WaitForHealthData());
    }

    IEnumerator WaitForHealthData()
    {
        while (GameManager.instance == null || GameManager.instance.healthData == null)
        {
            yield return null; // Wait until healthData is assigned
        }
        GameManager.instance.healthData.OnUpdateDDefaultValue += OnUpdateHealth;
        OnUpdateHealth();
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        if (GameManager.instance != null && GameManager.instance.healthData != null)
        {
            GameManager.instance.healthData.OnUpdateDDefaultValue -= OnUpdateHealth;
        }
    }




    public void OnUpdateHealth()
    {
        health = Mathf.RoundToInt(GameManager.instance.healthData.DefaultValue);
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
        if (EventSystem.current.IsPointerOverGameObject() || !LevelManager.instance.isGameStarted)
            return;



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
    public void ThrowGrenade()
    {
        if (ThrowableGranadeData.throwablePrefab == null || throwingPoint == null) return;
        MyDebug.Log($"Throwing Grenade.");

        // Instantiate the grenade at the throwing point
        GameObject grenade = Instantiate(ThrowableGranadeData.throwablePrefab, throwingPoint.position, Quaternion.identity);

        // Get the Rigidbody2D of the grenade to apply force and torque (spin)
        if (grenade.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            // Calculate the 45-degree direction
            float angleInRadians = 45 * Mathf.Deg2Rad;
            Vector2 throwDirection = new(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

            // Apply a forward force to throw the grenade
            rb2d.AddForce(throwDirection * ThrowableGranadeData.throwableThrowForce, ForceMode2D.Impulse);

            // Apply a random spin to the grenade (adjust the value for more or less spin)
            float spinForce = Random.Range(-ThrowableGranadeData.throwableSpinForce, ThrowableGranadeData.throwableSpinForce);
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

        if (enemyWaveGenerator.enemyList.Count > 0)
        {
            foreach (var enemy in enemyWaveGenerator.enemyList)
            {
                if (enemy == null) continue;

                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = enemy.transform.position;
                    closestDistance = distance;
                }
            }
            MyDebug.Log($"Closet Enemy Pos:{closestEnemy}");
            return closestEnemy;
        }
        else
        {
            return Vector3.zero;
        }
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
        if (!isPlayerAlive) return;
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
        isPlayerAlive = false;
        // Handle  death (e.g., despawn, play death animation)
        healthBar.KillHealthTween();
        MyDebug.Log("Player died!");
        LevelManager.instance.OnPlayerDeath();
        Destroy(gameObject);
        // Handle player death (respawn or game over logic)
    }


    public Transform GetTransform()
    {
        return transform;
    }

    public void ActivateFirePowerdMode()
    {
        currentWeapon.ActivatePoweredMode();
    }
}
