using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Scripting.APIUpdating;

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
    public float movementDirection = -1;

    [Header("Shake Effect on Attack")]

    public Zombie enemyInFront;  // Enemy directly in front
    public Zombie enemyBehind;   // Enemy directly behind
    public Zombie enemyOnTop;    // Enemy stacked on top




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
                MoveDirection();
            }
        }
        else if (closestTarget != null)
        {
            // Calculate distance to the closest target
            var distance = Vector3.Distance(transform.position, closestTarget.GetTransform().position);

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


    public override void OnDetectEnemyInFront(GameObject enemyObj)
    {

        StartCoroutine(PerformJumpSequence());
    }

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

    // /// <summary>
    // /// Coroutine to make the zombie jump on top of the specific enemy object using Rigidbody2D.
    // /// </summary>
    // private void JumpToEnemy(GameObject enemyObj)
    // {
    //     hasJumped = true;

    //     if (enemyObj != null)
    //     {
    //         // Calculate the direction from current position to the target enemy position
    //         Vector2 direction = (enemyObj.transform.position - transform.position).normalized;

    //         // Apply upward and forward force towards the enemy's position
    //         Vector2 jumpDirection = new Vector2(direction.x, 1f).normalized;  // Adding upward force

    //         // Apply force to the Rigidbody2D for jumping
    //         rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

    //         // Start a coroutine to check when the zombie has reached the target
    //         // StartCoroutine(HandleLandingOnTarget(enemyObj));
    //     }
    // }

    /// <summary>
    /// Moves the zombie upwards by 0.3 units using DOTween.
    /// </summary>
    private void MoveUpByFixedValue()
    {
        canMoveForward = false;
        // Target position is the current position with the Y increased by 0.3 units
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

        // Use DOTween to animate the zombie upwards
        transform.DOMoveY(targetPosition.y, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                canMoveForward = true;
                // Logic after movement completes, if necessary
            });
    }


    /// <summary>
    /// Moves the zombie upwards by applying a force to the Rigidbody2D.
    /// </summary>
    private void MoveUpByForce()
    {
        // Ensure there's a Rigidbody2D attached
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Apply upward force to move the zombie, adjusting the force magnitude to achieve approximately 0.3 units of movement
            float forceAmount = 2f; // Adjust this value based on the mass and gravity scale of the Rigidbody2D
            rb.AddForce(Vector2.up * forceAmount, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("No Rigidbody2D found on this object.");
        }
    }


    /// <summary>
    /// Animates the zombie jump to the enemy's jumpPosition using a curved path with DOTween.
    /// </summary>
    private void JumpToEnemy(GameObject enemyObj)
    {


        if (enemyObj != null && !hasJumped)
        {
            hasJumped = true;
            // Get the Enemy component from the target object
            Zombie enemyComponent = enemyObj.GetComponent<Zombie>();

            if (enemyComponent != null)
            {
                // Get the target jump position from the enemy component
                Vector3 targetJumpPosition = enemyComponent.jumpPoint.position;

                // Calculate a mid-point above the current and target position to create a curved arc
                Vector3 midPoint = (transform.position + targetJumpPosition) / 2;  // Mid-point between start and end
                midPoint.y += .1f;  // Adjust the Y position to simulate the jump arc

                // Create a path using the current position, mid-point, and target position for a curved jump
                Vector3[] path = { transform.position, midPoint, targetJumpPosition };

                // Animate the zombie along the curved path
                transform.DOPath(path, .7f, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        // Mark the jump as complete
                        hasJumped = false;

                        // Assign this zombie as the one on top of the enemy
                        enemyComponent.SetEnemyOnTop(this);
                    });
            }
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

    /// <summary>
    /// Move this zombie backwards.
    /// </summary>
    public void MoveBackward()
    {
        movementDirection = 1;
    }

    // /// <summary>
    // /// Detect when this zombie collides with another.
    // /// </summary>
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.TryGetComponent<Zombie>(out var otherZombie))
    //     {
    //         Debug.Log("Found a zoombie");

    //         if (enemyBehind == null)
    //         {
    //             SetEnemyBehind(otherZombie);
    //             otherZombie.SetEnemyInFront(this);
    //         }
    //     }
    // }
    #endregion
}
