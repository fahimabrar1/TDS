using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour, IEnemyDamagable, IAttackable
{
    [Tooltip("The initial health of the enemy.")]
    public int Health { get; set; } = 50;  // Initial zombie health
    public HealthBar healthBar;

    [SerializeField]
    protected Transform target;

    [SerializeField]
    protected bool isAttacking = false;

    protected CircleCollider2D detectionCollider;  // Collider for detection

    // A list to track all targets in range
    protected List<IPlayerDamagable> targetsInRange = new List<IPlayerDamagable>();



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Initialize the detection collider (assumed to be attached to the zombie)
        detectionCollider = GetComponent<CircleCollider2D>();

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
        healthBar.InitializeHealthBar(Health);

    }



    public virtual void OnTakeDamage(int damage)
    {
        healthBar.DeduceHealth(damage);
        // currentHealth -= damage;
        // showHealthBar = true;
        healthBar.ShowHealthbar();
        MyDebug.Log($"Zombie took {damage} damage. Health remaining: {healthBar.currentHealth}");

        if (healthBar.currentHealth <= 0)
        {
            Die();
        }
        else
        {
            healthBar.UpdateHealthbar();
        }
    }

    public virtual void OnAttack(IDamagable target)
    {
        int attackDamage = 15;  // Example damage value
        target.OnTakeDamage(attackDamage);
        MyDebug.Log("Zombie attacked!");
    }

    private void Die()
    {
        MyDebug.Log("Zombie has died!");

        // Handle zombie death (e.g., despawn, play death animation)
        healthBar.KillHealthTween();

        Destroy(gameObject);  // Destroy the zombie when dead
    }




    /// <summary>
    /// Coroutine for repeatedly attacking the closest target with a bounce animation.
    /// </summary>
    /// <param name="damagableTarget">The target to attack.</param>
    public abstract IEnumerator AttackRoutine(IDamagable damagableTarget);

    /// <summary>
    /// Detects when a player enters the attack zone and initiates the attack routine on the closest one.
    /// </summary>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Add valid player targets to the list
        if (other.TryGetComponent<IPlayerDamagable>(out var playerTarget))
        {
            targetsInRange.Add(playerTarget);
            CheckAndAttackClosestTarget();
        }
    }

    /// <summary>
    /// Called when a trigger collider exits the detection zone.
    /// Removes the target from the list and checks if another one should be attacked.
    /// </summary>
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        // Remove the target from the list when it exits the trigger
        if (other.TryGetComponent<IPlayerDamagable>(out var playerTarget))
        {
            targetsInRange.Remove(playerTarget);
            CheckAndAttackClosestTarget();
        }
    }

    /// <summary>
    /// Finds the closest target and initiates the attack.
    /// </summary>
    protected void CheckAndAttackClosestTarget()
    {
        if (targetsInRange.Count == 0)
        {
            // No targets in range, stop attacking
            isAttacking = false;
            return;
        }

        // Find the closest target
        IPlayerDamagable closestTarget = FindClosestTarget();

        if (!isAttacking && closestTarget != null)
        {
            // If not already attacking, start attacking the closest target
            StartCoroutine(AttackRoutine(closestTarget));
            isAttacking = true;
        }
    }

    /// <summary>
    /// Finds the closest IPlayerDamagable target in range.
    /// </summary>
    /// <returns>The closest IPlayerDamagable target.</returns>
    protected IPlayerDamagable FindClosestTarget()
    {
        IPlayerDamagable closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var target in targetsInRange)
        {
            float distance = Vector2.Distance(transform.position, ((MonoBehaviour)target).transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}