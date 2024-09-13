using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using System;

public class Zombie : Enemy
{

    [Header("Move Data")]
    public float forwardSpeed = 2f; // Initial movement speed
    public float backwardSpeed = .2f; // Initial movement speed
    public float stopDistance = 0.18f; // Distance to completely stop
    public float brakingFactor = 0.5f; // Speed reduction factor when braking
    public float stopThreshold = 0.1f; // Minimum speed threshold for stopping
    public float acceleration = 2f; // Acceleration rate


    [Header("Attack Data")]
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





    [Header("Debug Values")]
    // [SerializeField]
    // private List<Collider2D> ignoreColliders = new();
    [SerializeField]
    private float currentForwardSpeed;


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    public void OnEnable()
    {
        OnSetInTopEnemyEvent += MoveBackward;
        OnMoveBackwardEvent += MoveBackward;
        OnMoveForwarwEvent += MoveForwad;
    }



    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    public void OnDisable()
    {
        OnSetInTopEnemyEvent -= MoveBackward;
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Start()
    {
        base.Start();
        frontCollider.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnter2DFront(col));
        frontCollider.OnTriggerExit2DEvent.AddListener((col) => OnTriggerExit2DFront(col));
        backCollfier.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnter2DBack(col));
        backCollfier.OnTriggerExit2DEvent.AddListener((col) => OnTriggerExit2DBack(col));
        bottomCollider.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnter2DBottom(col));
        bottomCollider.OnTriggerExit2DEvent.AddListener((col) => OnTriggerExit2DBottom(col));

        currentForwardSpeed = forwardSpeed; // Set current speed to the initial move speed
    }


    void FixedUpdate()
    {
        if (canMove)
        {
            if (EnemyInFront != null && !EnemyInFront.isInAir)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, EnemyInFront.transform.position);

                MyDebug.Log("Detected Enemy: " + EnemyInFront.name + " at distance: " + distanceToEnemy);

                // If the enemy is within stop distance, stop movement
                if ((distanceToEnemy - stopDistance) < 0.1f)
                {
                    currentForwardSpeed = 0f; // Stop completely
                    canMove = false; // Disable movement
                    // ignoreColliders.Add(EnemyInFront.boxCollider2D); // Ignore this collider from now on


                    // Call Climb function if needed
                    if (EnemyInFront.EnemyOnTop == null && EnemyOnTop == null)
                    {
                        EnemyOnBottom = EnemyInFront;
                        MyDebug.Log($"Nulling {EnemyInFront.name} by {gameObject.name}");
                        EnemyInFront = null;
                        EnemyOnBottom.SetEnemyOnTop(this);

                        Climb(new Vector3(EnemyOnBottom.transform.position.x, EnemyOnBottom.transform.position.y + (Mathf.Abs(capsuleCollider2D.offset.y) + capsuleCollider2D.size.y / 4) / 2, EnemyOnBottom.transform.position.z));
                    }
                    else
                    {
                        canMove = false;
                    }
                }  // else
                   // {
                   //     // If the enemy is within detectDistance but further than stopDistance, start braking
                   //     float brakeFactor = Mathf.Clamp01((distanceToEnemy - stopDistance) / stopDistance);
                   //     currentSpeed = moveSpeed * brakeFactor; // Slow down based on proximity
                   // }
            }
            // Gradually increase speed to the target moveSpeed
            if (currentForwardSpeed < forwardSpeed)
            {
                currentForwardSpeed += acceleration * Time.fixedDeltaTime;
                if (currentForwardSpeed > forwardSpeed)
                {
                    currentForwardSpeed = forwardSpeed;
                }
            }

            // Move the zombie to the left
            MoveToDirection();
        }
    }


    public void Climb(Vector3 targetPosition)
    {
        isInAir = true;
        // Create a path with a curve
        Vector3[] path = {
            transform.position,
            new((transform.position.x + targetPosition.x) / 2, transform.position.y + 0.15f, transform.position.z),
            targetPosition
        };

        // Disable physics during the animation to avoid conflicts
        rb.isKinematic = true;
        rb.velocity = Vector2.zero; // Stop any velocity during the animation

        // Use DOTween to animate the path
        transform.DOPath(path, 1, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                isInAir = false;
                // After completing the path, reset the position
                transform.position = targetPosition;
                // Re-enable physics and start accelerating
                rb.isKinematic = false;
                rb.velocity = Vector2.zero; // Reset velocity to avoid sudden jumps

                // Set canMove to true to allow movement after the climb
                canMove = true;
                EnemyOnBottom.OnMoveBackwardEvent.Invoke();
            });
    }


    private void MoveToDirection()
    {
        float newSpeed = 0;
        if (moveDirection == -1)
            newSpeed = backwardSpeed * moveDirection;
        else
            newSpeed = currentForwardSpeed * moveDirection;
        // Apply the calculated speed to move the enemy left
        rb.velocity = newSpeed * Vector2.left;
    }


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

    /// <summary>
    /// Creates a shake effect using DOTween when the zombie attacks.
    /// </summary>
    private void ShakeOnAttack()
    {
        body.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                .SetEase(Ease.OutQuad);
    }

    #region Collierrs
    private void OnTriggerEnter2DFront(Collider2D col)
    {
        // Detect when another enemy enters the front collider
        // if (col.CompareTag("Enemy") && !ignoreColliders.Contains(col))
        if (col.CompareTag("Enemy"))
        {
            if (col.gameObject.TryGetComponent(out Enemy mate) && col.gameObject.layer != LayerMask.NameToLayer("IgnorableEnemyCollider"))
            {
                MyDebug.Log($"On Ad Front Enemt for {gameObject.name} by {mate.gameObject.name}");
                EnemyInFront = mate;
                if (EnemyBehind != null)
                    EnemyBehind.OnMoveForwarwEvent?.Invoke();
            }
        }
        else
        {
            base.OnTriggerEnterFront2D(col);
        }

    }
    private void OnTriggerExit2DFront(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.gameObject.TryGetComponent(out Enemy mate) && col.gameObject.layer != LayerMask.NameToLayer("IgnorableEnemyCollider"))
            {
                MyDebug.Log($"On Remove Front Enemt for {gameObject.name} by {mate.gameObject.name}");
                if (mate == EnemyInFront)
                {
                    OnMoveForwarwEvent?.Invoke();
                }
            }
        }
    }





    private void OnTriggerEnter2DBack(Collider2D col)
    {
        // Detect when another enemy enters the front collider
        // if (col.CompareTag("Enemy") && !ignoreColliders.Contains(col))
        if (col.CompareTag("Enemy"))
        {
            if (col.gameObject.TryGetComponent(out Enemy mate))
            {

                MyDebug.Log($"{gameObject.name} EnemyOnBottom == mate: {EnemyBehind == mate}, (EnemyOnBottom == null && moveDirection == -1): {(EnemyBehind == null && moveDirection == -1)}");
                if (EnemyBehind == mate || (EnemyBehind == null && moveDirection == 1))
                {
                    MyDebug.Log($"On Ad Requesting to move back");
                    mate.OnMoveBackwardEvent?.Invoke();
                }
                MyDebug.Log($"On Ad Back Enemt for {gameObject.name} by {mate.gameObject.name} ");

                EnemyBehind = mate;
                if (EnemyBehind.EnemyOnTop == this)
                    EnemyBehind.EnemyOnTop.SetEnemyOnTop(null);



            }
        }
    }

    private void OnTriggerExit2DBack(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.gameObject.TryGetComponent(out Enemy mate))
            {
                if (EnemyBehind == mate)
                    EnemyBehind = null;
            }
        }
    }





    private void OnTriggerExit2DBottom(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            isInAir = false;
            // EnemyOnBottom = null;
        }
        else if (col.CompareTag("Enemy"))

        {
            if (col.gameObject.TryGetComponent(out Enemy mate))
            {
                if (mate == EnemyOnBottom)
                {
                    isInAir = true;
                    EnemyOnBottom = null;
                }
            }
        }

    }

    private void OnTriggerEnter2DBottom(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            // isInAir = true;
        }
        else if (col.CompareTag("Enemy"))

        {
            if (col.gameObject.TryGetComponent(out Enemy mate))
            {
                if (mate == EnemyOnBottom)
                {
                    isInAir = false;
                    EnemyOnBottom = mate;
                }
            }
        }
    }


    #endregion
}
