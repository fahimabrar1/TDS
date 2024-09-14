using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Enemy : MonoBehaviour, IEnemyDamagable, IAttackable
{

    [Header("Health Data")]
    [Tooltip("The initial health of the enemy.")]
    public int Health = 50;  // Initial zombie health


    public HealthBar healthBar;

    [Header("Assign Data")]

    [SerializeField]
    protected Transform body;

    [SerializeField]
    public Transform jumpPoint;

    [SerializeField]
    protected Transform target;


    [Header("Custom Colliders")]

    public BoxCollider2D boxCollider2D;
    public CapsuleCollider2D capsuleCollider2D;
    public MyCollider2D frontCollider;
    public MyCollider2D backCollfier;
    public MyCollider2D bottomCollider;



    // A list to track all targets in range
    protected List<IPlayerDamagable> targetsInRange = new();
    protected Coroutine attackEnumurator;
    protected CircleCollider2D detectionCollider;  // Collider for detection
    [Header("Debug Values")]

    [SerializeField]
    protected Rigidbody2D rb;
    public int moveDirection = 1;
    [SerializeField]
    protected bool isGrounded = false;
    public bool canMove = true;
    public bool isInAir = false;


    [SerializeField]
    protected bool isAttacking = false;

    [SerializeField]
    private Enemy enemyInFront;  // Enemy directly in front

    // Property to safely manipulate coins
    public Enemy EnemyInFront
    {
        get => enemyInFront;
        protected set
        {
            enemyInFront = value;
            // OnSetInFrontEnemyEvent?.Invoke();
        }
    }

    [SerializeField]
    private Enemy enemyBehind;   // Enemy directly behind

    // Property to safely manipulate coins
    public Enemy EnemyBehind
    {
        get => enemyBehind;
        protected set
        {
            enemyBehind = value;
            // OnSetInBehindEnemyEvent?.Invoke();
        }
    }

    [SerializeField]
    private Enemy enemyOnTop;    // Enemy stacked on top

    // Property to safely manipulate coins
    public Enemy EnemyOnTop
    {
        get => enemyOnTop;
        set
        {
            enemyOnTop = value;
            // OnSetInTopEnemyEvent?.Invoke();
        }
    }


    [SerializeField]
    private Enemy enemyOnBottom;    // Enemy stacked on bottom

    // Property to safely manipulate coins
    public Enemy EnemyOnBottom
    {
        get => enemyOnBottom;
        protected set
        {
            enemyOnBottom = value;
            // OnSetInBottomEnemyEvent?.Invoke();
        }
    }


    public Action OnMoveForwarwEvent;
    public Action OnSetInTopEnemyEvent;
    public Action OnMoveBackwardEvent;





    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D

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

        // // front collider
        // frontCollier.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));
        // frontCollier.OnTriggerExit2DEvent.AddListener(OnTriggerExitFront2D);
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
            attackEnumurator = StartCoroutine(AttackRoutine(closestTarget));
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
            if (target == null)
            {
                targetsInRange.Remove(target);
                continue;
            }
            float distance = Vector2.Distance(transform.position, target.GetTransform().position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }





    public Transform GetTransform()
    {
        return transform;
    }


    #region Colliders

    /// <summary>
    /// Detects when a player enters the attack zone and initiates the attack routine on the closest one.
    /// </summary>
    public virtual void OnTriggerEnterFront2D(Collider2D other)
    {
        MyDebug.Log($"Entered {other.name}");
        // Add valid player targets to the list
        if (other.TryGetComponent(out IPlayerDamagable playerTarget))
        {
            canMove = true;
            targetsInRange.Add(playerTarget);
            CheckAndAttackClosestTarget();
        }

    }

    /// <summary>
    /// Called when a trigger collider exits the detection zone.
    /// Removes the target from the list and checks if another one should be attacked.
    /// </summary>
    public virtual void OnTriggerExitFront2D(Collider2D other)
    {

        MyDebug.Log($"Exited {other.name}");
        // Remove the target from the list when it exits the trigger
        if (other.TryGetComponent(out IPlayerDamagable playerTarget))
        {
            targetsInRange.Remove(playerTarget);
            if (attackEnumurator != null)
                StopCoroutine(attackEnumurator);
            CheckAndAttackClosestTarget();
        }
    }




    public virtual void OnDetectEnemyInFront(GameObject enemyObj)
    {

    }

    public virtual void MoveBackward()
    {
        if (EnemyBehind != null)
            EnemyBehind.OnMoveBackwardEvent?.Invoke();
        canMove = true;
        moveDirection = -1;
        isAttacking = false;
    }

    public virtual void MoveForwad()
    {
        if (enemyBehind == null)
        {
            moveDirection = 1;
            canMove = true;
        }
    }

    public virtual void SetEnemyOnTop(Enemy mate)
    {
        EnemyOnTop = mate;
    }

    #endregion
}