using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Zombie : Enemy
{
    [Header("Move Data")]
    public float forwardSpeed = 2f; // Initial movement speed
    public float backwardSpeed = .2f; // Initial movement speed
    public float stopDistance = 0.18f; // Distance to completely stop
    public float brakingFactor = 0.5f; // Speed reduction factor when braking
    public float stopThreshold = 0.1f; // Minimum speed threshold for stopping
    public float acceleration = 2f; // Acceleration rate
    public float raycastDistance = 1f; // How far to check for other enemies

    [Header("Attack Data")]
    public float attackDelay = 1f;

    [Header("Shake Effect on Attack")]
    public float shakeDuration = 0.2f;
    public Vector3 shakeStrength = new Vector3(0.3f, 0.3f, 0);
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;

    [Header("Debug Values")]
    [SerializeField]
    private float currentForwardSpeed;

    private void OnEnable()
    {
        OnSetInTopEnemyEvent += MoveBackward;
        OnMoveBackwardEvent += MoveBackward;
        OnMoveForwarwEvent += MoveForwad;
    }

    private void OnDisable()
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
        currentForwardSpeed = forwardSpeed; // Set current speed to the initial move speed
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            RaycastForEnemies();

            // Gradually increase speed to the target moveSpeed only when there is no enemy in front
            if (EnemyInFront == null || Vector2.Distance(transform.position, EnemyInFront.transform.position) > stopDistance)
            {
                if (currentForwardSpeed < forwardSpeed)
                {
                    currentForwardSpeed += acceleration * Time.fixedDeltaTime;
                    if (currentForwardSpeed > forwardSpeed)
                    {
                        currentForwardSpeed = forwardSpeed;
                    }
                }
            }

            // Move the zombie
            MoveToDirection();
        }
    }

    /// <summary>
    /// Performs raycasts to detect enemies in front, behind, and below using RaycastAll.
    /// </summary>
    private void RaycastForEnemies()
    {
        // Raycast in front
        RaycastHit2D[] hitsFront = Physics2D.RaycastAll(transform.position, Vector2.left, raycastDistance, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(transform.position, Vector3.left * raycastDistance, Color.white);

        // Filter out self and find the closest enemy in front
        foreach (RaycastHit2D hit in hitsFront)
        {

            if (hit.collider.TryGetComponent(out Enemy mate))
            {
                if (mate != this)
                {
                    MyDebug.Log("Detected Enemy in Front: " + mate.name);
                    HandleFrontEnemy(mate);
                }
            }
        }

        // Raycast behind
        RaycastHit2D[] hitsBack = Physics2D.RaycastAll(transform.position, Vector2.right, raycastDistance, LayerMask.GetMask("Enemy"));
        Debug.DrawRay(transform.position, Vector3.right * raycastDistance, Color.white);

        foreach (RaycastHit2D hit in hitsBack)
        {

            if (hit.collider.TryGetComponent(out Enemy mateBehind))
            {
                if (mateBehind != this)
                {
                    MyDebug.Log("Detected Enemy Behind: " + mateBehind.name);
                    HandleBackEnemy(mateBehind);
                }
            }
        }

        // Raycast below
        RaycastHit2D[] hitsBottom = Physics2D.RaycastAll(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Ground", "Enemy"));
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.white);

        bool foundGround = false;
        foreach (RaycastHit2D hit in hitsBottom)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isInAir = false;
                foundGround = true;
                MyDebug.Log("Zombie is on the ground");
            }
            else if (hit.collider.CompareTag("Enemy") && hit.collider.TryGetComponent(out Enemy mateBelow))
            {
                if (mateBelow != this)
                {
                    MyDebug.Log("Detected Enemy Below: " + mateBelow.name);
                    HandleBottomEnemy(mateBelow);
                }
            }
        }

        if (!foundGround)
        {
            isInAir = true;
        }
    }


    private void HandleFrontEnemy(Enemy mate)
    {
        EnemyInFront = mate;
        float distanceToEnemy = Vector2.Distance(transform.position, EnemyInFront.transform.position);
        MyDebug.Log($"Distance between {transform.name}, {EnemyInFront.transform.name} :{distanceToEnemy}");
        // If the enemy is within the range of 0.4f and 0.2f, start slowing down

        // Stop the enemy when within stopDistance (0.2f)
        if (distanceToEnemy - stopDistance < .05f)
        {
            currentForwardSpeed = 0f;
            canMove = false;

            // If no one is on top, perform climb
            if (EnemyInFront.EnemyOnTop == null && EnemyOnTop == null)
            {
                EnemyOnBottom = EnemyInFront;
                EnemyInFront = null;
                EnemyOnBottom.EnemyOnTop = this;
                Climb(new Vector3(EnemyOnBottom.transform.position.x, EnemyOnBottom.transform.position.y +
                    (Mathf.Abs(capsuleCollider2D.offset.y) + capsuleCollider2D.size.y / 4) / 2, EnemyOnBottom.transform.position.z));
            }
        }
        else if (distanceToEnemy <= raycastDistance && distanceToEnemy >= stopDistance)
        {
            // Linearly reduce speed based on the distance (slows down as it gets closer)
            float t = (distanceToEnemy - stopDistance) / (raycastDistance - stopDistance); // Normalizes between 0.4f and 0.2f
            currentForwardSpeed = Mathf.Lerp(0f, forwardSpeed, t);
        }
    }



    private void HandleBackEnemy(Enemy mate)
    {
        if (EnemyOnBottom == mate) return;
        EnemyBehind = mate;
        if (EnemyBehind.EnemyOnTop == this)
            EnemyBehind.EnemyOnTop.SetEnemyOnTop(null);
    }

    private void HandleBottomEnemy(Enemy mate)
    {
        EnemyOnBottom = mate;
    }

    private void MoveToDirection()
    {
        float newSpeed = moveDirection == -1 ? backwardSpeed * moveDirection : currentForwardSpeed * moveDirection;
        rb.velocity = newSpeed * Vector2.left;
    }

    public override IEnumerator AttackRoutine(IDamagable damagableTarget)
    {
        isAttacking = true;
        while (target != null && targetsInRange.Count != 0)
        {
            if (damagableTarget != null)
            {
                OnAttack(damagableTarget);
                ShakeOnAttack();
            }
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
    }

    private void ShakeOnAttack()
    {
        body.DOShakeScale(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness).SetEase(Ease.OutQuad);
    }

    public void Climb(Vector3 targetPosition)
    {
        isInAir = true;
        Vector3[] path = {
            transform.position,
            new((transform.position.x + targetPosition.x) / 2, transform.position.y + 0.15f, transform.position.z),
            targetPosition
        };

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        transform.DOPath(path, 1, PathType.CatmullRom).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            isInAir = false;
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            canMove = true;
            EnemyOnBottom.OnMoveBackwardEvent?.Invoke();
        });
    }


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            MyDebug.Log("Triggerted Enter");
        }
    }
}
