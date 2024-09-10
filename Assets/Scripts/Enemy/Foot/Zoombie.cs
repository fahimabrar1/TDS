using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Zombie : Enemy
{
    [Tooltip("The speed at which the zombie moves towards the player or crate.")]
    public float moveSpeed = 2f;


    [Tooltip("Time delay between each attack in seconds.")]
    public float attackRange = 1f;

    [Tooltip("Time delay between each attack in seconds.")]
    public float attackDelay = 1f;

    [Header("Shake Effect on Attack")]
    [Tooltip("The duration of the shake effect when the zombie attacks.")]
    public float shakeDuration = 0.2f;

    [Tooltip("The strength of the shake in each axis.")]
    public Vector3 shakeStrength = new Vector3(0.3f, 0.3f, 0);

    [Tooltip("The number of shakes during the attack effect.")]
    public int shakeVibrato = 10;

    [Tooltip("The randomness factor for the shake.")]
    public float shakeRandomness = 90f;


    public bool canMoveForward = false;

    [Header("Shake Effect on Attack")]

    public Zombie enemyInFront;  // Enemy directly in front
    public Zombie enemyBehind;   // Enemy directly behind
    public Zombie enemyOnTop;    // Enemy stacked on top

    public override void Start()
    {
        Health = 100;
        canMoveForward = true;
        healthBar.InitializeHealthBar(Health);
        // Find player position from the LevelManager
        target = LevelManager.instance.playerTransform;
    }

    private void Update()
    {
        // Find the closest target
        var closestTarget = FindClosestTarget();

        // If there are no targets or the closest target is null
        if (closestTarget == null && canMoveForward)
        {
            // Move forward if not attacking
            if (!isAttacking)
            {
                MoveForward();
            }
        }
        else if (closestTarget != null)
        {
            // Calculate distance to the closest target
            var distance = Vector3.Distance(transform.position, closestTarget.GetTransform().position);

            // If the distance is greater than the attack range and the zombie is not attacking, move forward
            if (distance > attackRange && !isAttacking && canMoveForward)
            {
                MoveForward();
            }

        }
    }



    private void FixedUpdate()
    {
        if (enemyInFront != null)
        {
            if (Vector3.Distance(transform.position, enemyInFront.transform.position) < attackRange)
            {
                canMoveForward = false;
            }
            else
            {
                canMoveForward = true;
            }
        }
    }

    /// <summary>
    /// Moves the zombie forward in the direction it is facing.
    /// </summary>
    private void MoveForward()
    {
        // Move in the direction the zombie is facing
        transform.Translate(moveSpeed * Time.deltaTime * -Vector2.right, Space.Self);
    }

    /// <summary>
    /// Creates a shake effect using DOTween when the zombie attacks.
    /// </summary>
    private void ShakeOnAttack()
    {
        transform.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                .SetEase(Ease.OutQuad);
    }



    /// <summary>
    /// Coroutine for repeatedly attacking the target with a bounce animation.
    /// </summary>
    /// <param name="damagableTarget">The target to attack.</param>
    public override IEnumerator AttackRoutine(IDamagable damagableTarget)
    {
        isAttacking = true;
        while (target != null && targetsInRange.Count != 0)
        {
            if (damagableTarget != null)
            {
                // Attack and bounce when in range
                OnAttack(damagableTarget);
                ShakeOnAttack();
            }
            // Wait for the attack delay before attacking again
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
    }





    #region Enemy Wave Effect


    /// <summary>
    /// Set the enemy directly in front of this one.
    /// </summary>
    public void SetEnemyInFront(Zombie enemy)
    {
        enemyInFront = enemy;
    }

    /// <summary>
    /// Set the enemy directly behind this one.
    /// </summary>
    public void SetEnemyBehind(Zombie enemy)
    {
        enemyBehind = enemy;
    }

    /// <summary>
    /// Set the enemy stacked on top of this one.
    /// </summary>
    public void SetEnemyOnTop(Zombie enemy)
    {
        enemyOnTop = enemy;
    }



    /// <summary>
    /// Handles the queue logic and communication between enemies.
    /// </summary>
    private void HandleQueue()
    {
        if (enemyInFront != null && enemyBehind != null)
        {
            // Check if the enemy in front has someone on top
            if (enemyInFront.enemyOnTop == null)
            {
                // Jump logic if there's no one on top
                StartCoroutine(JumpOnTop());
            }
            else
            {
                // No jump; move backwards if needed
                MoveBackward();
                enemyBehind.MoveBackward(); // Signal the enemy behind to move back
            }
        }
    }

    [Button("Jump On Top")]
    /// <summary>
    /// Coroutine to make the zombie jump on top of the one in front.
    /// </summary>
    private IEnumerator JumpOnTop()
    {
        if (enemyInFront != null && enemyInFront.enemyOnTop == null)
        {
            // Jump animation using DOTween
            transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad);

            // Assign this zombie to the top of the one in front
            enemyInFront.SetEnemyOnTop(this);

            yield return new WaitForSeconds(1f); // Wait before falling off

            // Fall off after a delay
            transform.DOMoveY(transform.position.y - 1f, 0.5f).SetEase(Ease.InQuad);
            enemyInFront.SetEnemyOnTop(this);
        }
    }

    /// <summary>
    /// Move this zombie backwards.
    /// </summary>
    public void MoveBackward()
    {
        transform.DOMoveX(transform.position.x - 1f, 0.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// Detect when this zombie collides with another.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Zombie>(out var otherZombie))
        {
            Debug.Log("Found a zoombie");

            if (enemyBehind == null)
            {
                SetEnemyBehind(otherZombie);
                otherZombie.SetEnemyInFront(this);
            }
        }
    }
    #endregion
}
