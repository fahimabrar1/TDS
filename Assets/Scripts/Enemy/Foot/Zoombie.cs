using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Scripting.APIUpdating;
using System;

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
    public bool hasJumped = false;

    public float jumpForce = 3;

    private IDamagable currentTarget;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        rb = GetComponent<Rigidbody2D>(); // Initialize Rigidbody2D
    }



    public override void Start()
    {
        Health = 100;
        canMoveForward = true;
        hasJumped = false;
        healthBar.InitializeHealthBar(Health);
        // Find player position from the LevelManager
        // target = LevelManager.instance.playerTransform;
    }

    private void Update()
    {
        // If there are no targets or the closest target is null
        if (currentTarget == null && canMoveForward)
        {
            // Move forward if not attacking
            if (!isAttacking)
            {
                MoveDirection();
            }
        }
        else if (currentTarget != null)
        {  // Find the closest target
            var closestTarget = FindClosestTarget();

            var distance = Mathf.Infinity;
            if (currentTarget == null)
            {
                // Calculate distance to the closest target
                distance = Vector3.Distance(transform.position, closestTarget.GetTransform().position);
            }

            // If the distance is greater than the attack range and the zombie is not attacking, move forward
            if (distance > attackRange && !isAttacking && canMoveForward)
            {
                MoveDirection();
            }

        }
    }



    private void FixedUpdate()
    {
        if (enemyInFront != null)
        {
            var distance = Vector3.Distance(transform.position, enemyInFront.transform.position);
            if (enemyInFront.enemyOnTop == null && distance < attackDelay)
            {
                canMoveForward = false;
                HandleQueue();
            }
            else
            {
                if (distance < attackRange)
                {
                    canMoveForward = false;

                }
                else
                {
                    canMoveForward = true;
                }
            }

        }
    }

    /// <summary>
    /// Moves the zombie forward in the direction it is facing.
    /// </summary>
    private void MoveDirection()
    {
        // Move in the direction the zombie is facing
        transform.Translate(moveSpeed * Time.deltaTime * movementDirection * Vector2.right, Space.Self);
    }

    /// <summary>
    /// Creates a shake effect using DOTween when the zombie attacks.
    /// </summary>
    private void ShakeOnAttack()
    {
        body.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                .SetEase(Ease.OutQuad);
    }



    /// <summary>
    /// Coroutine for repeatedly attacking the target with a bounce animation.
    /// </summary>
    /// <param name="damagableTarget">The target to attack.</param>
    public override IEnumerator AttackRoutine(IDamagable damagableTarget)
    {
        currentTarget = damagableTarget;
        isAttacking = true;
        while (targetsInRange.Count != 0)
        {
            if (damagableTarget.GetTransform() != null)
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


    public override void OnDetectEnemyInFront(GameObject enemyObj)
    {

        StartCoroutine(PerformJumpSequence());
    }





    /// <summary>
    /// Perform a sequence of movements: move up, jump forward, then move forward.
    /// </summary>
    private IEnumerator PerformJumpSequence()
    {
        if (!hasJumped)
        {
            hasJumped = true;
            // Move up
            Vector3 initialPosition = transform.position;
            Vector3 upwardPosition = initialPosition + new Vector3(0, 0.3f, 0);
            float moveDuration = 0.2f; // Duration of the move up

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, upwardPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = upwardPosition;

            // Perform jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Wait for a bit to let the jump complete
            yield return new WaitForSeconds(0.2f);

            hasJumped = true;
            movementDirection = -1;
            // Move forward
            MoveDirection();
        }



    }

    /// <summary>
    /// Handles the queue logic and communication between enemies.
    /// </summary>
    private void HandleQueue()
    {
        if (enemyInFront != null)
        {
            // Check if the enemy in front has someone on top
            if (enemyInFront.enemyOnTop == null)
            {
                // Jump logic if there's no one on top
                if (!hasJumped)
                    JumpToEnemy();
            }
            else
            {
                // No jump; move backwards if needed

                MoveBackward();
                enemyBehind?.MoveBackward(); // Signal the enemy behind to move back
            }
        }
    }

    [Button("Jump On Enemy")]
    /// <summary>
    /// Coroutine to make the zombie jump on top of the one in front using Rigidbody2D.
    /// </summary>
    private void JumpToEnemy()
    {
        hasJumped = true;

        if (enemyInFront != null && enemyInFront.enemyOnTop == null)
        {
            // Apply upward force for the jump using Rigidbody2D
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Add upward impulse force

            // Assign this zombie to the top of the one in front after reaching peak height
            StartCoroutine(HandleTopPosition());
        }
    }




    /// <summary>
    /// Coroutine to handle positioning once the zombie reaches the top of the enemy object.
    /// </summary>
    private IEnumerator HandleLandingOnTarget(GameObject enemyObj)
    {
        // Wait for the zombie to reach a peak height or the target
        yield return new WaitForSeconds(0.5f);  // You can tweak this delay depending on the jump speed

        // Snap the zombie's position on top of the enemy object
        if (enemyObj != null)
        {
            // Adjust the Y position slightly above the enemy to simulate landing on top
            transform.position = new Vector3(enemyObj.transform.position.x, enemyObj.transform.position.y + 1f, transform.position.z);

            // Set the enemy as the object on top
            enemyObj.GetComponent<Zombie>().SetEnemyOnTop(this);
        }

        // End the jump
        hasJumped = false;
    }



    /// <summary>
    /// Handles the assignment of the enemy to the top after the jump.
    /// </summary>
    private IEnumerator HandleTopPosition()
    {
        yield return new WaitForSeconds(0.5f); // Wait for a short time to simulate jump duration (adjust if needed)

        // Set this enemy on top of the front enemy after jump
        if (enemyInFront != null)
        {
            enemyInFront.SetEnemyOnTop(this);
        }

        hasJumped = false; // Reset jump flag
    }



    #endregion
}
